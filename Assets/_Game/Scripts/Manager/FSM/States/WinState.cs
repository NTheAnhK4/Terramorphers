using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using GameCore.Presentation.Shared;
using GameCore.Usecase.Level;

using VContainer;

namespace Terramorphers
{
    public class WinState : GameState
    {
        private TransitionService _transitionService;
        private ILevelRepository _levelRepository;
        
        private LevelUseCase _levelUseCase;

        [Inject]
        public void Constructor(TransitionService transitionService, ILevelRepository levelRepository, LevelUseCase levelUseCase)
        {
            _transitionService = transitionService;
            _levelRepository = levelRepository;
            _levelUseCase = levelUseCase;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            UnlockNextLevel();
            EnterAsync().Forget();
        }

        private async UniTask EnterAsync()
        {
            var winGamePresentor = await _transitionService.ShowWinGameModal();
        }

        private void UnlockNextLevel()
        {
            var levelModel = _levelUseCase.GetModel();
            ILevelDatabase levelDatabase = _levelRepository.Get();
            int currentLevelID = levelModel.SelectedLevel;
            int currentStageID = levelModel.SelectedStage + 1;
            var levelMetaData = levelDatabase.GetByType(currentLevelID);

            if (currentStageID >= levelMetaData.LevelStageDatas.Count)
            {
                currentLevelID++;
                if (currentLevelID >= levelDatabase.DataCount) return;
                currentStageID = 0;
            }

            levelModel.CurrentLevel = currentLevelID;
            _levelUseCase.SetCurrentStageOfLevel(currentLevelID, currentStageID);
            _levelUseCase.Update(levelModel).Forget();

        }
    }
}