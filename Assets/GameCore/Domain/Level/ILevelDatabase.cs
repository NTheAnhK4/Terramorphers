using System.Collections;
using System.Collections.Generic;
using GameCore.Domain.Shared;
using UnityEngine;
namespace GameCore.Domain.Level{
    public interface ILevelDatabase : IMasterDatabase<int, LevelMetadata>
    {
        int DataCount { get; }
    }
}

