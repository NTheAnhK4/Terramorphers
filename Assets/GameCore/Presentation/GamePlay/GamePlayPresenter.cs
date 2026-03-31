using System;
using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Domain.Skill;
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

        [Inject]
        public void Constructor(ICommandPublisher publisher, ICommandSubscribable subscribable, ISkillRepository skillRepository, SkillUseCase skillUseCase, IObjectResolver resolver)
        {
            _publisher = publisher;
            _subscribabale = subscribable;
            _skillRepository = skillRepository;
            _skillUseCase = skillUseCase;
            _resolver = resolver;
        }

        public GamePlayPresenter(GamePlayScreen view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, GamePlayViewState state, GamePlayScreen view)
        {
            _state = state;
            _subscribabale.Subscribe<EnableEndTurnCommand>(ToggleEndTurnButton).AddTo(view);
            _subscribabale.Subscribe<IncreaseRoundCommand>(SetRound).AddTo(view);
            state.EndTurnCommand.Subscribe(OnEndTurnBtnClick).AddTo(view);

            //Skill
            _skillDatabase = _skillRepository.Get();
            _skillModel = _skillUseCase.GetModel();
            foreach (var skillId in _skillModel.CurrentSkills)
            {
                state.SkillMetadatas.Add(_skillDatabase.GetByType(skillId));
            }
            view.InitSkillView(_resolver, state.SkillMetadatas);

            return base.Initialize(args, state, view);
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
    }
}