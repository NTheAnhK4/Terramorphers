using R3;
using WEngine.MVP;

namespace GameCore.Presentation.WinGame
{
    public class WinGameViewState : ViewState
    {
        public ReactiveCommand ToMenuCommand { get; } = new();
        public ReactiveCommand NextLevelCommand { get; } = new();
        public ReactiveProperty<int> TotalStars { get; } = new();
    }

}
