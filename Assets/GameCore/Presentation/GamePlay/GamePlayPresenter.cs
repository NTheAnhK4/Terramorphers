using System;
using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Domain.Skill;
using GameCore.Presentation.Shared;
using GameCore.Usecase.Skill;
using VContainer;
using VitalRouter;
using WEngine.MVP;
using R3;
using UnityEngine;


namespace GameCore.Presentation.GamePlay
{
    public class GamePlayPresenter : ScreenPresenter<GamePlayScreen, GamePlayViewState>
    {
        private ICommandPublisher _publisher;
        private ICommandSubscribable _subscribabale;
        private GamePlayViewState _state;
        private ISkillRepository _skillRepository;
        private SkillUseCase _skillUseCase;
        private ISkillDatabase _skillDatabase;
        private SkillModel _skillModel;
        private IObjectResolver _resolver;
        private TransitionService _transitionService;
        public ReactiveProperty<bool> IsShowingUI { get; } = new();
        

        [Inject]
        public void Constructor(ICommandPublisher publisher, ICommandSubscribable subscribable
            , ISkillRepository skillRepository, SkillUseCase skillUseCase, IObjectResolver resolver
            ,TransitionService transitionService)
        {
            _publisher = publisher;
            _subscribabale = subscribable;
            _skillRepository = skillRepository;
            _skillUseCase = skillUseCase;
            _resolver = resolver;
            _transitionService = transitionService;
        }

        public GamePlayPresenter(GamePlayScreen view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, GamePlayViewState state, GamePlayScreen view)
        {
           
            _state = state;
            _subscribabale.Subscribe<EnableEndTurnCommand>(ToggleEndTurnButton).AddTo(view);
            _subscribabale.Subscribe<IncreaseRoundCommand>(SetRound).AddTo(view);
            _subscribabale.Subscribe<ChangePlayerStaminaCommand>(OnStaminaChange).AddTo(view);
            _subscribabale.Subscribe<ChangePlayerManaCommand>(OnManaChange).AddTo(view);

         
           
            state.EndTurnCommand.Subscribe(OnEndTurnBtnClick).AddTo(view);
            state.ObjectiveCommand.Subscribe(_ => ShowObjectives().Forget()).AddTo(view);
            state.ExitCommand.Subscribe(_ => OnExit().Forget()).AddTo(view);
           
            //Skill
            _skillDatabase = _skillRepository.Get();
            _skillModel = _skillUseCase.GetModel();
            foreach (var skillId in _skillModel.CurrentSkills)
            {
                state.SkillMetadatas.Add(_skillDatabase.GetByType(skillId));
            }
         
            InitAppView();
            return base.Initialize(args, state, view);
        }

        private void InitAppView()
        {
           var skillInfoPresenter =  View.InitSkillInfoPresenter();
           _resolver.Inject(skillInfoPresenter);
           skillInfoPresenter.Initialize();

           for (int i = 0; i < View.SkillViews.Count; ++i)
           {
               var skillView = View.SkillViews[i];
               if (i >= _state.SkillMetadatas.Count) skillView.gameObject.SetActive(false);
               else
               {
                   skillView.gameObject.SetActive(true);
                   SkillViewPresenter skillViewPresenter = new SkillViewPresenter(skillView, _state.SkillMetadatas[i], skillInfoPresenter);
                   _resolver.Inject(skillViewPresenter);
                   skillViewPresenter.Initialize();
               }
           }
        }
     

        private void OnStaminaChange(ChangePlayerStaminaCommand command, PublishContext context)
        {
            _state.Stamina.Value = (command.Stamina, command.MaxStamina);
        }

        private void OnManaChange(ChangePlayerManaCommand command, PublishContext context)
        {
            _state.Mana.Value = (command.Mana, command.MaxMana);
        }

        private void SetRound(IncreaseRoundCommand command, PublishContext context) => _state.CurrentRound.Value = command.NewRound;


        private void OnEndTurnBtnClick(Unit _)
        {
            _publisher.PublishAsync(new EndEntityTurnCommand());
        }

        private void ToggleEndTurnButton(EnableEndTurnCommand command, PublishContext context)
        {
            _state.IsActiveEndTurnCommand.Value = command.IsEnable;
        }

       

        private async UniTask ShowObjectives()
        {
            try
            {
                IsShowingUI.Value = true;
                var presentor = await _transitionService.ShowStageObjectiveModal();
                await UniTask.WaitUntil(() => presentor.IsClose, cancellationToken: View.GetCancellationTokenOnDestroy());
                IsShowingUI.Value = false;
            }
            catch(OperationCanceledException){}
           
        }

        private async UniTask OnExit()
        {
            try
            {
                IsShowingUI.Value = true;
                var menuPresentor = await _transitionService.ShowMenuModal();
                await UniTask.WaitUntil(() => menuPresentor.IsContinue, cancellationToken: View.GetCancellationTokenOnDestroy());
            }
            catch(OperationCanceledException){}
         
        }
    }
}