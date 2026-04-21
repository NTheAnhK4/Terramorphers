using GameCore.Domain.Reward;
using GameCore.Respository.Shared;

namespace GameCore.Respository.Reward
{
    public class RewardItemRepository : BaseRepository<ERewardItemType, RewardItemMetadata, IRewardItemDatabase>, IRewardItemRepository
    {
        public override string AddressPath => "RewardItemDatabase";
    }
}