using R3;
using WEngine.MVP;

namespace GameCore.Presentation.LoseGame
{
    public class LoseGameViewState : ViewState
    {
        public ReactiveCommand ToMenuCommand { get; } = new ReactiveCommand();
        public ReactiveCommand RetryCommand { get; } = new ReactiveCommand();
    }
}

