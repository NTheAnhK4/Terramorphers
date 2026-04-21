using System;
using UnityEngine;

namespace GameCore.Domain.Reward
{
    [Serializable]
    public class RewardItemMetadata
    {
        [SerializeField] private Sprite rewardSprite;

        public Sprite RewardSprite => rewardSprite;
    }
}