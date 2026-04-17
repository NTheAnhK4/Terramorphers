using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Utility;
using UnityEngine;
using WEngine.MVP;
using UnityEngine.UI;
using R3;
namespace GameCore.Presentation.WinGame
{
    
    
    public class WinGameModal : Modal<WinGameViewState>
    {
        [SerializeField] private Button menuBtn;
        [SerializeField] private Button nextBtn;
        public override UniTask InitializeState(WinGameViewState state, Memory<object> args)
        {
            menuBtn.SubscribeToCommand(state.ToMenuCommand).AddTo(this);
            nextBtn.SubscribeToCommand(state.NextLevelCommand).AddTo(this);
            return UniTask.CompletedTask;
        }
    }

}
