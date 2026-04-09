using System.Collections;
using System.Collections.Generic;
using Terramorphers;
using UnityEngine;

namespace UtilityAI.ActionDataBuilder
{
    public abstract class ActionDataBuilder : ScriptableObject
    {
        public abstract void Build(ActionExecutionData data);
    }
    public class ActionExecutionData
    {
        public int SkillID;
        public ITile MoveTile;
        public ITile ApplySkillTile;
    }
}

