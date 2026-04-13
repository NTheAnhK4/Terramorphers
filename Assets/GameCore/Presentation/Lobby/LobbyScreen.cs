using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WEngine.MVP;

namespace GameCore.Presentation.Lobby{
    public class LobbyScreen : Screen<LobbyViewState>
    {
        public override UniTask InitializeState(LobbyViewState state, Memory<object> args)
        {
            return UniTask.CompletedTask;
        }
    }

}
