using R3;
using WEngine.MVP;

namespace GameCore.Presentation.ChooseStage
{
    public class StageViewState : ViewState
    {
        public ReactiveProperty<bool> IsUnlock { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> StageName { get; } = new ReactiveProperty<string>();
        public ReactiveCommand EnterStageCommand { get; } = new ReactiveCommand();
        public int TotalStars { get; set; } 
    }
}