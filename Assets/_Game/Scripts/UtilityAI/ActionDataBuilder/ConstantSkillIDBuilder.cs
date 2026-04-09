using UnityEngine;

namespace UtilityAI.ActionDataBuilder
{
    public class ConstantSkillIDBuilder : ActionDataBuilder
    {
        [SerializeField] private int skillID;
        public override void Build(ActionExecutionData data)
        {
            data.SkillID = skillID;
        }
    }
}