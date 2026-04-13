using GameCore.Domain.Shared;

namespace GameCore.Domain.Tile
{
    public interface ITileDatabase : IMasterDatabase<ETileType, TileMetadata>
    {
        
    }
}