using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Presentation.Shared;
using UnityEngine;

namespace Terramorphers
{
    public class LobbyState : GameState
    {
        private readonly TransitionService _transitionService;
        private readonly GameManager _gameManager;

        public LobbyState(GameManager gameManager, TransitionService transitionService)
        {
            _gameManager = gameManager;
            _transitionService = transitionService;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            ShowLobbyScreen().Forget();
        }
        private async  UniTask ShowLobbyScreen()
        {
            var lobbyPresenter = await _transitionService.ShowLobbyScreen();
        }
            
    }

}
