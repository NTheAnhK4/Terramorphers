using Cysharp.Threading.Tasks;
using R3;
using TMPro;
using UnityEngine;
using WEngine.MVP;

namespace GameCore.Presentation.Currency.Gold
{
    public class GoldView : AppView<GoldViewState>
    {
        [SerializeField] private TextMeshProUGUI amountText;
        protected override UniTask Initialize(GoldViewState state)
        {
            state.Amount.Subscribe(SetAmount).AddTo(this);
            SetAmount(state.Amount.Value);
            return UniTask.CompletedTask;
        }

        private void SetAmount(int amount) => amountText.text = $"{amount}";
    }
}