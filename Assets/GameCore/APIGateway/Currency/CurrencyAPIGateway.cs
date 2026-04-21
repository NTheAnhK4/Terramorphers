using GameCore.Domain.Currency;

namespace GameCore.APIGateway.Currency
{
    public class CurrencyAPIGateway : BaseAPIGateway<CurrencyModel>
    {
        protected override string PlayerPrefsKey => "CurrencyData";
      

        protected override CurrencyModel CreateDefaultModel()
        {
            return new CurrencyModel() { TotalGold = 0 };
        }
    }
}