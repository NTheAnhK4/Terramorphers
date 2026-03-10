
using GameCore.Domain.Shared;
using UnityEngine;

namespace Terramorphers
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "Database/LevelDatabase")]
    public class LevelDatabase : BaseDatabase<int, LevelMetadata>
    {
        
    }

}
