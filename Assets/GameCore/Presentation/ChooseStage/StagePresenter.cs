using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using GameCore.Usecase.Level;
using VContainer;
using WEngine.MVP;

namespace GameCore.Presentation.ChooseStage
{
    public class StagePresenter : AppViewPresenter<StageView, StageViewState>
    {
        [Inject] private LevelUseCase _levelUseCase;
        private LevelStageData _levelStageData;
        private int _stageID;
        private int _levelID;
        public StagePresenter(StageView view,int levelID, int stageID, LevelStageData levelStageData) : base(view)
        {
            _levelStageData = levelStageData;
            _stageID = stageID;
            _levelID = levelID;
        }

        protected override UniTask Initialize(StageViewState state, StageView view)
        {
            state.IsUnlock.Value = _stageID <= _levelUseCase.GetCurrentStageOfLevel(_levelID);
            state.StageName.Value = $"Stage {_stageID + 1}";
            return UniTask.CompletedTask;
        }
    }
}