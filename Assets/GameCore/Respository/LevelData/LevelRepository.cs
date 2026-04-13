using GameCore.Domain.Level;
using GameCore.Respository.Shared;

namespace GameCore.Respository.LevelData
{
    public class LevelRepository : BaseRepository<int, LevelMetadata, ILevelDatabase>, ILevelRepository
    {
        public override string AddressPath => "LevelDatabase";
    }
}