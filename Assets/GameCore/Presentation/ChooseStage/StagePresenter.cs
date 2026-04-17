using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Domain.Entity;
using GameCore.Domain.Level;
using R3;
using GameCore.Usecase.Level;
using VContainer;
using VitalRouter;
using WEngine.MVP;

namespace GameCore.Presentation.ChooseStage
{
    public class StagePresenter : AppViewPresenter<StageView, StageViewState>
    {
        [Inject] private LevelUseCase _levelUseCase;
        [Inject] private IEntityRepository _entityRepository;
        [Inject] private ICommandPublisher _publisher;
       
        private IEntityDatabase _entityDatabase;
        private LevelStageData _levelStageData;
        private int _stageID;
        private int _levelID;
        private ReactiveCommand _onClose;
        public StagePresenter(StageView view,int levelID, int stageID, LevelStageData levelStageData, ReactiveCommand onClose) : base(view)
        {
            _levelStageData = levelStageData;
            _stageID = stageID;
            _levelID = levelID;
            _onClose = onClose;
        }

        protected override UniTask Initialize(StageViewState state, StageView view)
        {
            state.IsUnlock.Value = _stageID <= _levelUseCase.GetCurrentStageOfLevel(_levelID);
            state.StageName.Value = $"Stage {_stageID + 1}";
            state.EnterStageCommand.Subscribe(OnEnterLevel).AddTo(view);
            _entityDatabase = _entityRepository.Get();
            List<BaseEntityMetadata> baseEntityMetadatas = new();
            foreach (var enemyID in _levelStageData.EnemyIDs)
            {
                baseEntityMetadatas.Add(_entityDatabase.GetByType(enemyID));
            }
            View.ShowEnemies(baseEntityMetadatas);
         
            return UniTask.CompletedTask;
        }

        private void OnEnterLevel(Unit _)
        {
            var levelModel = _levelUseCase.GetModel();
            levelModel.SelectedLevel = _levelID;
            levelModel.SelectedStage = _stageID;
            _onClose.Execute(_);
            _publisher.PublishAsync(new ChangeGameStateTypeCommand(EGameStateType.LoadingState));
        }
    }
}