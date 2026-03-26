using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Presentaion.Shared;
using UnityEngine;
using VitalRouter;

namespace Terramorphers
{
    public class LoadingState : GameState
    {
        private LevelDatabase _levelDatabase;
        private EntityManager _entityManager;
        private GameManager _gameManager;
        private BoardManager _boardManager;
        private ICommandPublisher _publisher;
        private TransitionService _transitionService;

        public LoadingState(LevelDatabase levelDatabase, EntityManager entityManager, GameManager gameManager, BoardManager boardManager, ICommandPublisher publisher,
            TransitionService transitionService)
        {
            _levelDatabase = levelDatabase;
            _gameManager = gameManager;
            _boardManager = boardManager;
            _publisher = publisher;
            _entityManager = entityManager;
            _transitionService = transitionService;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            LoadLevelAsync().Forget();
        }

        private async UniTask LoadLevelAsync()
        {
            if (_gameManager.GameMode == GameMode.Unknown) return;
            var gamePresenter = await _transitionService.ShowGamePlayScreen();
            int level = PlayerPrefs.GetInt(string.Format(GameConstant.LEVEL_PLAYER_PREFS, _gameManager.GameMode), 0);
            LevelMetadata levelMetadata = _levelDatabase.GetByType(level);

            if (levelMetadata == null) return;
            bool isLoadingBoardFinished = await _boardManager.LoadingBoard(levelMetadata);
            if (!isLoadingBoardFinished) return;

            List<ITile> passibleTile = _boardManager.GetPassableTile();
            if (passibleTile == null)
            {
                Debug.Log($"[LoadingState] no passible tile in board");
                return;
            }

            //Load Player
            ITile playerTile = GetRandomTile(passibleTile);
            await _entityManager.AddEntity(0, playerTile);

            ITile enemyTile = GetRandomTile(passibleTile);
            await _entityManager.AddEntity(1, enemyTile);

            //TODO: show anim
            switch (_gameManager.GameMode)
            {
                case GameMode.AdvantureMode:
                    await _publisher.PublishAsync(new ChangeGameStateTypeCommand(EGameStateType.AdvantureMode));
                    break;
            }
        }

        private ITile GetRandomTile(List<ITile> tiles)
        {
            if (tiles == null || tiles.Count == 0) return null;
            int id = Random.Range(0, tiles.Count);
            ITile tile = tiles[id];
            tiles.RemoveAt(id);
            return tile;
        }
    }
}