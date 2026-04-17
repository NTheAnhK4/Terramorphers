using GameCore.Domain.Entity;
using GameCore.Domain.Shared;
using UnityEngine;

namespace GameCore.Respository.Entity
{
    [CreateAssetMenu(fileName = "EntityDatabase", menuName = "Database/EntityDatabase")]
    public class EntityDatabase : BaseDatabase<int, BaseEntityMetadata>, IEntityDatabase
    {
        
    }
}