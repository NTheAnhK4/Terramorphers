using System;

using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Domain.Level;
using GameCore.Presentation.Shared;
using GameCore.Usecase.Level;
using GameCore.Utility;
using WEngine.MVP;
using R3;
using VContainer;
using VitalRouter;

namespace GameCore.Presentation.Menu
{
    public class MenuPresenter :  ModalPresenter<MenuModal, MenuViewState>
    {
        private TransitionService _transitionService;
        private ICommandPublisher _publisher;
        private LevelUseCase _levelUseCase;
        private ILevelRepository _levelRepository;
        public bool IsContinue { get; private set; }

        [Inject]
        public void Constructor(TransitionService transitionService, ICommandPublisher publisher,
            LevelUseCase levelUseCase, ILevelRepository levelRepository)
        {
            _transitionService = transitionService;
            _publisher = publisher;
            _levelUseCase = levelUseCase;
            _levelRepository = levelRepository;
        }
        public MenuPresenter(MenuModal view) : base(view)
        {
        }

        protected override UniTask Initialize(Memory<object> args, MenuViewState state, MenuModal view)
        {
            IsContinue = false;
            state.CloseCommand.Subscribe(OnClose).AddTo(View);
            state.RestartCommand.Subscribe(OnRestart).AddTo(View);
            state.QuitCommand.Subscribe(_ => QuitAsync().Forget()).AddTo(View);
            return UniTask.CompletedTask;
        }

        private void OnClose(Unit _)
        {
            IsContinue = true;
            _transitionService.ClosePopup();
        }

        private void OnRestart(Unit _)
        {
            _publisher.PublishAsync(new ChangeGameStateTypeCommand(EGameStateType.LoadingState));
            _transitionService.ClosePopup();
        }

        private async UniTask QuitAsync()
        {
            await  _transitionService.ClosePopup();
            async UniTask OnFinishLoading()
            {
                var levelModel = _levelUseCase.GetModel();
                var _levelDatabase = _levelRepository.Get();
                int currentLevelID = levelModel.CurrentLevel;
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