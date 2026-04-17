using System.Collections;
using System.Collections.Generic;
using R3;
using UnityEngine;
using WEngine.MVP;

namespace GameCore.Presentation.ChooseStage
{
    public class ChooseStageViewState : ViewState
    {
        public ReactiveProperty<string> LevelName { get; } = new ReactiveProperty<string>();
        public ReactiveCommand OnClose { get; } = new ReactiveCommand();
    }

}
