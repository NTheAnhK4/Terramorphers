using System;
using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Domain.Level;
using GameCore.Presentation.Shared;
using GameCore.Usecase.Level;
using VContainer;
using VitalRouter;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.LoseGame
{
    public class LoseGamePresentor : ModalPresenter<LoseGameModal, LoseGameViewState>
    {
        private LevelUseCase _levelUseCase;
        private LevelModel _levelModel;
        private ILevelRepository _levelRepository;
        private ILevelDatabase _levelDatabase;
        private ICommandPublisher _publisher;
        private TransitionService _transitionService;
        private int currentLevelID;
      

        [Inject]
        public void Constructor(LevelUseCase levelUseCase, ILevelRepository levelRepository,
            ICommandPublisher publisher, TransitionService transitionService)
        {
            _levelUseCase = levelUseCase;
            _levelRepository = levelRepository;
            _publisher = publisher;
            _transitionService = transitionService;
        }
        public LoseGamePresentor(LoseGameModal view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, LoseGameViewState state, LoseGameModal view)
        {
            state.RetryCommand.Subscribe(RetryLevel).AddTo(view);
            state.ToMenuCommand.Subscribe(_ => ToMenu().Forget()).AddTo(view);
            _levelModel = _levelUseCase.GetModel();
            currentLevelID = _levelModel.SelectedLevel;
           
            _levelDatabase = _levelRepository.Get();
            return UniTask.CompletedTask;
        }

        private void RetryLevel(Unit _)
        {
            _publisher.PublishAsync(new ChangeGameStateTypeCommand(EGameStateType.LoadingState));
            _transitionService.ClosePopup();
        }
        private async UniTask ToMenu()
        {
            await  _transitionService.ClosePopup();
            async UniTask OnFinishLoading()
            {
                await _transitionService.ShowChooseStageModal(currentLevelID, _levelDatabase.GetByType(currentLevelID));
            }
            ReactiveProperty<float> progress = new ReactiveProperty<float>();
            var loadingPresenter = await _transitionService.ShowLoadingView(true, progress, () => OnFinishLoading().Forget());
            await loadingPresenter.GetPreLoading();
            
            await loadingPresenter.GetLoading();
            await _publisher.PublishAsync(new ChangeGameStateTypeCommand(EGameStateType.LobbyState));
            progress.Value = 1;

            
        }
    }
}