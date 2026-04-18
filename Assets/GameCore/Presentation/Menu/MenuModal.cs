using System;
using Cysharp.Threading.Tasks;
using GameCore.Utility;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.Menu
{
    public class MenuModal : Modal<MenuViewState>
    {
        [SerializeField] private Button closeBtn, continueBtn, restartBtn, quitBtn;
        public override UniTask InitializeState(MenuViewState state, Memory<object> args)
        {
            closeBtn.SubscribeToCommand(state.CloseCommand).AddTo(this);
            continueBtn.SubscribeToCommand(state.CloseCommand).AddTo(this);
            restartBtn.SubscribeToCommand(state.RestartCommand).AddTo(this);
            quitBtn.SubscribeToCommand(state.QuitCommand).AddTo(this);
            return UniTask.CompletedTask;
        }
    }
}