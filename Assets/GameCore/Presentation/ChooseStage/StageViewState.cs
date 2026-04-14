using R3;
using WEngine.MVP;

namespace GameCore.Presentation.ChooseStage
{
    public class StageViewState : ViewState
    {
        public ReactiveProperty<bool> IsUnlock { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> StageName { get; } = new ReactiveProperty<string>();
    }
}