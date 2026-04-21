using Cysharp.Threading.Tasks;
using GameCore.Usecase.Currency;
using GameCore.Utility;
using VContainer;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.Currency.Gold
{
    public class GoldPresenter : AppViewPresenter<GoldView, GoldViewState>
    {
        [Inject] private CurrencyUseCase _currencyUseCase;
        public GoldPresenter(GoldView view) : base(view)
        {
        }

        protected override UniTask Initialize(GoldViewState state, GoldView view)
        {
            var currencyModel = _currencyUseCase.GetModel();
            currencyModel.Gold.SubscribeToReactiveProperty(state.Amount).AddTo(view);
            return UniTask.CompletedTask;
        }
    }
}