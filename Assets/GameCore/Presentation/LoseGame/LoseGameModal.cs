using System;
using Cysharp.Threading.Tasks;
using GameCore.Utility;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.LoseGame
{
    public class LoseGameModal : Modal<LoseGameViewState>
    {
        [SerializeField] private Button menuButton;
        [SerializeField] private Button retryButton;
        public override UniTask InitializeState(LoseGameViewState state, Memory<object> args)
        {
            menuButton.SubscribeToCommand(state.ToMenuCommand).AddTo(this);
            retryButton.SubscribeToCommand(state.RetryCommand).AddTo(this);
            return UniTask.CompletedTask;
        }
    }
}