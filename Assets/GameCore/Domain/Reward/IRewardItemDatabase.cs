using GameCore.Domain.Shared;

namespace GameCore.Domain.Reward
{ 
    public interface IRewardItemDatabase : IMasterDatabase<ERewardItemType, RewardItemMetadata>
    {
        
    }
}