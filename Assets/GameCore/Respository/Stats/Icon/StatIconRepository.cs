using GameCore.Domain.Stats;
using GameCore.Domain.Stats.Icon;
using GameCore.Respository.Shared;
using UnityEngine;

namespace GameCore.Respository.Stats.Icon
{
    public class StatIconRepository : BaseRepository<EStatsType, StatIconMetadata, IStatIconDatabase>, IStatIconRepository
    {
        public override string AddressPath => "StatIconDatabase";
    }
}