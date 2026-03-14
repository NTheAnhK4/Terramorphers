
using GameCore.Domain.Shared;
using UnityEngine;


namespace Terramorphers
{
    [CreateAssetMenu(fileName = "EntityDatabase", menuName = "Database/EntityDatabase")]
    public class EntityDatabase : BaseDatabase<int, EntityMetadata>
    {
    }

}
