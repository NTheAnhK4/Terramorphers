using GameCore.Domain.Reward;
using GameCore.Domain.Shared;
using UnityEngine;

namespace GameCore.Respository.Reward
{
    [CreateAssetMenu(fileName = "RewardItemDatabase", menuName = "Database/RewardItemDatabase")]
    public class RewardItemDatabase : BaseDatabase<ERewardItemType, RewardItemMetadata>, IRewardItemDatabase
    {
        
    }
}