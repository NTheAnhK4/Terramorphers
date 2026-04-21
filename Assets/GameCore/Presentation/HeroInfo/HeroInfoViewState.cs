
using R3;
using WEngine.MVP;

namespace GameCore.Presentation.HeroInfo
{
    public class HeroInfoViewState : ViewState
    {
        public ReactiveCommand ExitCommand { get; } = new ReactiveCommand();
        public ReactiveCommand HandleSelectionCommand { get; } = new ReactiveCommand();
        public ReactiveCommand<int> PreviewSkillCommand { get; } = new ReactiveCommand<int>();
        public ReactiveCommand<int> SelectSkillCommand { get; } = new();
        public ReactiveCommand<int> UnselectSkillCommand { get; } = new();
        public ReactiveCommand HidePanelCommand { get; } = new();
    }

}
