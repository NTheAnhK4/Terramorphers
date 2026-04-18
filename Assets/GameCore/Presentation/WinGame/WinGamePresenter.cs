using System;
using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Domain.Level;
using GameCore.Presentation.Shared;
using GameCore.Usecase.Level;
using WEngine.MVP;
using R3;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace GameCore.Presentation.WinGame
{
    public class WinGamePresenter : ModalPresenter<WinGameModal, WinGameViewState>
    {
        private LevelUseCase _levelUseCase;
        private LevelModel _levelModel;
        private ILevelRepository _levelRepository;
        private ILevelDatabase _levelDatabase;
        private ICommandPublisher _publisher;
        private TransitionService _transitionService;
        private int currentLevelID;
        private int currentStageID;
        private int _totalStars;
        [Inject]
        public void Constructor(LevelUseCase levelUseCase, ILevelRepository levelRepository,
            ICommandPublisher publisher, TransitionService transitionService)
        {
            _levelUseCase = levelUseCase;
            _levelRepository = levelRepository;
            _publisher = publisher;
            _transitionService = transitionService;
        }
        public WinGamePresenter(WinGameModal view, int totalStars) : base(view)
        {
            _totalStars = totalStars;
        }

        protected override UniTask Initialize(Memory<object> args, WinGameViewState state, WinGameModal view)
        {
            state.NextLevelCommand.Subscribe(NextLevel).AddTo(view);
          
            state.ToMenuCommand.Subscribe(_ => ToMenu().Forget()).AddTo(view);
            _levelModel = _levelUseCase.GetModel();
            currentLevelID = _levelModel.SelectedLevel;
            currentStageID = _levelModel.SelectedStage;
            _levelDatabase = _levelRepository.Get();
            state.TotalStars.Value = _totalStars;
            return UniTask.CompletedTask;
        }

        private void NextLevel(Unit _)
        {
            
           
            int stageID = currentStageID + 1;
            int levelID = currentLevelID;
            var levelMetadata = _levelDatabase.GetByType(levelID);
            if (stageID >= levelMetadata.LevelStageDatas.Count)
            {
                levelID++;
                if (levelID >= _levelDatabase.DataCount)
                {
                    ToMenu().Forget();
                    return;
                }
                stageID = 0;
            }

            _levelModel.SelectedLevel = levelID;
            _levelModel.SelectedStage = stageID;
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