using GameCore.Domain.Shared;
using UnityEngine;

namespace Terramorphers
{
    [CreateAssetMenu(fileName = "TileDatabase", menuName = "Database/TileDatabase")]
    public class TileDatabase : BaseDatabase<ETileType, TileMetadata>
    {
        
    }
}