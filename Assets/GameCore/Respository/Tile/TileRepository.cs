using GameCore.Domain.Tile;
using GameCore.Respository.Shared;

namespace GameCore.Respository.Tile
{
    public class TileRepository : BaseRepository<ETileType, TileMetadata, ITileDatabase>, ITileRepository
    {
        public override string AddressPath => "TileDatabase";
    }
}