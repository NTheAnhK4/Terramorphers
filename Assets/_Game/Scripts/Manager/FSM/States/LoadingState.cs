using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Domain.Level;
using GameCore.Presentation.Shared;
using GameCore.Usecase.Level;
using GameCore.Usecase.Quest;
using GameCore.Utility.Shape;
using R3;
using UnityEngine;
using VitalRouter;

namespace Terramorphers
{
    public class LoadingState : GameState
    {
        
        private EntityManager _entityManager;
        private GameManager _gameManager;
        private BoardManager _boardManager;
        private ICommandPublisher _publisher;
        private TransitionService _transitionService;
        private ILevelRepository _levelRepository;
        private LevelUseCase _levelUseCase;
        private QuestUseCase _questUseCase;
        private ReactiveProperty<float> progress = new();
        private UniTask loadingTask;

        public LoadingState( EntityManager entityManager, GameManager gameManager, BoardManager boardManager,
            ICommandPublisher publisher, TransitionService transitionService, ILevelRepository levelRepository,
            LevelUseCase levelUseCase, QuestUseCase questUseCase)
        {
            _levelRepository = levelRepository;
            _gameManager = gameManager;
            _boardManager = boardManager;
            _publisher = publisher;
            _entityManager = entityManager;
            _transitionService = transitionService;
            _levelUseCase = levelUseCase;
            _questUseCase = questUseCase;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Run().Forget();
        }

       

        private async UniTask Run()
        {
            progress = new ReactiveProperty<float>();
            var loadingPresenter = await _transitionService.ShowLoadingView(true, 
                progress, 
                () => _publisher.PublishAsync(new ChangeGameStateTypeCommand(EGameStateType.AdvantureMode)));

            await loadingPresenter.GetPreLoading();
            
            var loadingTask = loadingPresenter.GetLoading();
            await UniTask.WhenAll(loadingTask, LoadLevelAsync());
            progress.Value = 1;
          
            
        }
        private async UniTask LoadLevelAsync()
        {
            _entityManager.ClearEntity();
            _boardManager.ClearBoard();
            var gamePresenter = await _transitionService.ShowGamePlayScreen();
            await UniTask.WaitUntil(() =>gamePresenter.IsInitialized);
          
            var levelStageData = await LoadLevelStageData();
            if (levelStageData == null) return;
            var mapData = LoadingMap(levelStageData);
            if (mapData == null) return;
            
            
            //add objectives
            foreach (var questMetadata in levelStageData.StageStarObjectives)
            {
                _questUseCase.AddQuest(questMetadata);
            }
            
            bool isLoadingBoardFinished = await _boardManager.LoadingBoard(mapData.Rows);
            if (!isLoadingBoardFinished)
            {
                Debug.Log($"[LoadingState] loading board not success");
                return;
            }
            _boardManager.SetBackground(levelStageData.Background);

            Dictionary<int, List<Vector2Int>> teamPositons = GetTeamPositionsMap(mapData.SlotDatas);
            
            //spawn player
            bool isSpawnPlayer = true;
            foreach (var teamPos in teamPositons)
            {
                if (isSpawnPlayer)
                {
                    ITile playerTile = GetTile(teamPos.Value[Random.Range(0, teamPos.Value.Count)]);
                    //select playerID later
                    await _entityManager.AddEntity(0, playerTile, teamPos.Key);
                    isSpawnPlayer = false;
                    continue;
                }
                else
                {
                    List<Vector2Int> slotData = teamPos.Value;
                    for (int i = 0; i < levelStageData.EnemyIDs.Count; ++i)
                    {
                        int enemyID = levelStageData.EnemyIDs[i];
                        if (i >= slotData.Count)
                        {
                            Debug.Log($"[LoadingState] Not enough slots for enemy");
                            return;
                        }

                        ITile enemyTile = GetTile(slotData[i]);
                        await _entityManager.AddEntity(enemyID, enemyTile, teamPos.Key);
                    }

                    break;
                }
            }
            
            _entityManager.ResetEntityID();
            
           
            await UniTask.Delay(200);
            //TODO: show anim
           
        }

        private async UniTask<LevelStageData> LoadLevelStageData()
        {
            var levelDatabase = _levelRepository.Get();
            var levelModel = _levelUseCase.GetModel();
            int selectedLevel = levelModel.SelectedLevel;
            int selectedStage = levelModel.SelectedStage;

            var levelMetaData = levelDatabase.GetByType(selectedLevel);
            if (levelMetaData == null)
            {
                Debug.Log($"[LoadingState] cannot load level metadata for {selectedLevel}");
                return null;
            }

            var levelStageData = levelMetaData.LevelStageDatas[selectedStage];
            if (levelStageData == null)
            {
                Debug.Log($"[LoadingState] cannot load level stage data for {selectedStage} and {selectedLevel}");
                return null;
            }

            return levelStageData;
        }

        private MapData LoadingMap(LevelStageData levelStageData)
        {
           

            if (levelStageData.StageMap == null)
            {
                Debug.Log($"[LoadingState] stageMap is null");
                return null;
            }

            MapData mapData = JsonUtility.FromJson<MapData>(levelStageData.StageMap.text);
            if (mapData == null) return null;
            return mapData;
        }

        private Dictionary<int, List<Vector2Int>> GetTeamPositionsMap(List<SpawnSlotData> slotDatas)
        {
            Dictionary<int, List<Vector2Int>> result = new();
            foreach (var slotData in slotDatas)
            {
                if (!result.ContainsKey(slotData.TeamID)) result[slotData.TeamID] = new();
                result[slotData.TeamID].Add(slotData.Position);
            }

            return result;
        }

        private ITile GetTile(Vector2Int pos) => _boardManager.HexaBoard.Get(new Cube(pos.x, pos.y, -pos.x - pos.y));

       
        
    }
}