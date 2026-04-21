using R3;

namespace GameCore.Domain.Currency
{
    public class CurrencyModel
    {
        public int TotalGold;
        public ReactiveProperty<int> Gold { get; } = new();
    }
}