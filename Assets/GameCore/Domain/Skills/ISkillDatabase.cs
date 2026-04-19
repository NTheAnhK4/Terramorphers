using System.Collections;
using System.Collections.Generic;
using GameCore.Domain.Shared;
using UnityEngine;

namespace GameCore.Domain.Skill
{
    public interface ISkillDatabase : IMasterDatabase<int, SkillMetadata>
    {
        int DataCount { get; }
    }

}

