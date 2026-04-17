using System.Collections;
using System.Collections.Generic;
using GameCore.Domain.Shared;
using UnityEngine;

namespace GameCore.Domain.Entity
{
    public interface IEntityDatabase : IMasterDatabase<int, BaseEntityMetadata>
    {
        
    }

}
