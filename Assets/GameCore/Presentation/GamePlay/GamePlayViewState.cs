using WEngine.MVP;
using R3;
namespace GameCore.Presentation.GamePlay
{
    public class GamePlayViewState : ViewState
    {
        public ReactiveCommand EndTurnCommand { get; } = new();
        public ReactiveProperty<bool> IsActiveEndTurnCommand { get; } = new();
        public ReactiveProperty<int> CurrentRound { get; } = new();
    }
}