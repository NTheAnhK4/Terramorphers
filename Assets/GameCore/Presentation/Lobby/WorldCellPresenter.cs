using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using GameCore.Presentation.Shared;
using GameCore.Usecase.Level;
using WEngine.MVP;
using R3;

using VContainer;

namespace GameCore.Presentation.Lobby
{
    public class WorldCellPresenter : AppViewPresenter<WorldCellView, WorldCellViewState>
    {
        [Inject] private TransitionService _transitionService;
        [Inject] private LevelUseCase _levelUseCase;
        private LevelModel _levelModel;
        private LevelMetadata _levelMetadata;
        private int _levelID;
        public WorldCellPresenter(WorldCellView view,int levelID, LevelMetadata levelMetadata) : base(view)
        {
            _levelMetadata = levelMetadata;
            _levelID = levelID;
        }

        protected override async UniTask Initialize(WorldCellViewState state, WorldCellView view)
        {
            state.LevelMetadata.Value = _levelMetadata;
            state.EnterWorldCommand.Subscribe(OnEnterWorld).AddTo(View);
            _levelModel = _levelUseCase.GetModel();
            state.IsLevelUnlock.Value = _levelID <= _levelModel.CurrentLevel;
           
        }

        private void OnEnterWorld(Unit _)
        {
            _transitionService.ShowChooseStageModal(_levelID,_levelMetadata).Forget();
        }
    }
}