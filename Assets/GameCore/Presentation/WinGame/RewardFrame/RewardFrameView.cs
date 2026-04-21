using Cysharp.Threading.Tasks;

using GameCore.Domain.Reward;
using R3;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;

namespace GameCore.Presentation.WinGame
{
    public class RewardFrameView : AppView<RewardFrameViewState>
    {
        [SerializeField, TabGroup("Components")] private Image frameImage;
        [SerializeField, TabGroup("Components")] private Image rewardImage;
        [SerializeField, TabGroup("Components")] private TextMeshProUGUI amountText;
       

        protected override UniTask Initialize(RewardFrameViewState state)
        {
            if(!state.IsShow.Value) gameObject.SetActive(false);
            else
            {
                gameObject.SetActive(true);
                state.Amount.Subscribe(SetAmount).AddTo(this);
                state.RewardSprite.Subscribe(SetSprite).AddTo(this);
              
                SetAmount(state.Amount.Value);
                SetSprite(state.RewardSprite.Value);
               
                SetAmount(state.Amount.Value);

            }
            return UniTask.CompletedTask;
        }

        private void SetAmount(int amount)
        {
            if(amount == 1) amountText.gameObject.SetActive(false);
            else
            {
                amountText.gameObject.SetActive(true);
                amountText.text = amount.ToString();
            }
        }

        private void SetSprite(Sprite sprite)
        {
            rewardImage.sprite = sprite;
        }

    
    }

}
