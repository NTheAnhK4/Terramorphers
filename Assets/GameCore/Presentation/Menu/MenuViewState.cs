using R3;
using WEngine.MVP;

namespace GameCore.Presentation.Menu
{
    public class MenuViewState : ViewState
    {
        public ReactiveCommand CloseCommand { get; } = new ReactiveCommand();
        public ReactiveCommand RestartCommand { get; } = new ReactiveCommand();
        public ReactiveCommand QuitCommand { get; } = new ReactiveCommand();
    }
}

