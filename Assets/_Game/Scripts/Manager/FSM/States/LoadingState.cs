using Cysharp.Threading.Tasks;
using UnityEngine;
using VitalRouter;

namespace Terramorphers
{
    public class LoadingState : GameState
    {
        private LevelDatabase _levelDatabase;
        private GameManager _gameManager;
        private BoardManager _boardManager;
        private ICommandPublisher _publisher;
        public LoadingState(LevelDatabase levelDatabase,GameManager gameManager, BoardManager boardManager, ICommandPublisher publisher)
        {
            _levelDatabase = levelDatabase;
            _gameManager = gameManager;
            _boardManager = boardManager;
            _publisher = publisher;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            LoadLevelAsync().Forget();


        }

        private async UniTask LoadLevelAsync()
        {
            if(_gameManager.GameMode == GameMode.Unknown) return;
            int level = PlayerPrefs.GetInt(string.Format(GameConstant.LEVEL_PLAYER_PREFS, _gameManager.GameMode), 0);
            LevelMetadata levelMetadata = _levelDatabase.GetByType(level);
            
            if (levelMetadata == null) return;
            bool isLoadingBoardFinished = await _boardManager.LoadingBoard(levelMetadata);
            if (!isLoadingBoardFinished) return;
            
            
            
            //TODO: show anim
            switch (_gameManager.GameMode)
            {
                case GameMode.AdvantureMode:
                    await _publisher.PublishAsync(new ChangeGameStateTypeCommand(EGameStateType.AdvantureMode));
                    break;
            }
            
        }
    }
}