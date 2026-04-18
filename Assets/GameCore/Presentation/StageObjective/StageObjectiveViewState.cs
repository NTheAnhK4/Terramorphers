using System.Collections.Generic;
using R3;
using WEngine.MVP;

namespace GameCore.Presentation.StageObjective
{
    public class StageObjectiveViewState : ViewState
    {
        public ReactiveCommand CloseCommand { get; } = new();
        public List<ReactiveProperty<string>> ObjectiveDescriptions { get; } = new();
        public List<ReactiveProperty<bool>> IsObjectiveFailed { get; } = new();
    }

}
