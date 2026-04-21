using Cysharp.Threading.Tasks;
using GameCore.APIGateway.Currency;
using GameCore.Domain.Currency;

namespace GameCore.Usecase.Currency
{
    public class CurrencyUseCase : BaseUseCase<CurrencyAPIGateway, CurrencyModel>
    {
        public CurrencyUseCase(CurrencyAPIGateway apiGateway) : base()
        {
            _apiGateway = apiGateway;
        }

        public override CurrencyModel GetModel()
        {
            var model = base.GetModel();
            model.Gold.Value = model.TotalGold;
            return model;
        }
        public void AddGold(CurrencyModel model, int amount)
        {
            model.Gold.Value += amount;
            model.TotalGold += amount;
            Update(model).Forget();
        }
    }
}