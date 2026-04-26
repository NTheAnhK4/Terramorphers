using GameCore.Domain.Level;
using GameCore.Domain.Reward;
using GameCore.Domain.Skill;
using GameCore.Presentation.Shared;
using GameCore.Utility.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GameCore.Presentation.ChooseStage.Reward
{
    public class RewardView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private HoldButton _holdButton;
        [SerializeField] private Image _image;
        [Inject] private TransitionService _transitionService;
        [Inject] private IRewardItemRepository _rewardItemRepository;
        [Inject] private ISkillRepository _skillRepository;

        public void Init(StageRewardData data)
        {
            int averageAmount = (data.RewardItemData.MaxAmount + data.RewardItemData.MinAmount) / 2;
            if (averageAmount <= 1) amountText.gameObject.SetActive(false);
            else amountText.text = averageAmount.ToString();
            
            if (data.RewardItemData.RewardItemType == ERewardItemType.Skill)
            {
                amountText.gameObject.SetActive(false);
                _image.sprite = _skillRepository.Get().GetByType(data.RewardItemData.SkillID).SkillSprite;
            }
            else
            {
                amountText.gameObject.SetActive(true);
                _image.sprite = _rewardItemRepository.Get().GetByType(data.RewardItemData.RewardItemType).RewardSprite;
            }
        }
    }
}