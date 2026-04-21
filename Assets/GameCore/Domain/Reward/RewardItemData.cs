using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCore.Domain.Reward
{
    [Serializable]
    public class RewardItemData
    {
        [SerializeField] private ERewardItemType rewardItemType;
        private bool IsSkill => rewardItemType == ERewardItemType.Skill;
        [ShowIf(nameof(IsSkill))]
        [SerializeField] private int skillID;

        public int MinAmount;
        public int MaxAmount;
        public float DropRate;

        public ERewardItemType RewardItemType => rewardItemType;

        public int SkillID => skillID;

        public int GetAmount()
        {
            if (Random.value < DropRate) return Random.Range(MinAmount, MaxAmount + 1);
            return 0;
        }
    }

}
