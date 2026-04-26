using R3;
using WEngine.MVP;

namespace GameCore.Presentation.ChooseStage.Reward
{
    public class AllRewardViewState : ViewState
    {
        public ReactiveCommand FinishShow { get; } = new();
    }
}