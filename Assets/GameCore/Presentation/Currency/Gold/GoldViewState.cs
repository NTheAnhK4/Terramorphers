using R3;
using WEngine.MVP;

namespace GameCore.Presentation.Currency.Gold
{
    public class GoldViewState : ViewState
    {
        public ReactiveProperty<int> Amount { get; } = new();
    }
}