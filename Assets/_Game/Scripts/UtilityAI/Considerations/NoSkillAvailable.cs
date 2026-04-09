
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UtilityAI.ActionDataBuilder;

namespace UtilityAI.Consideration
{
    [CreateAssetMenu(menuName = "UtilityAI/Consideration/NoSkillAvailable", fileName = "NoSkillAvailable")]
    public class NoSkillAvailable : Consideration
    {
        public override float Evaluate(Context context, ActionExecutionData executionData)
        {
            if (context.Entity.SkillIDs == null || context.Entity.SkillIDs.Count == 0) return 1;
            int currentMana = context.GetData<int>(BlackBoardConstant.REMAIN_MANA_KEY);
            var skillManager = context.Entity.SkillManager;
            List<int> availableSkills = context.Entity.SkillIDs.
                Select(t => skillManager.GetSkillMetadata(t).SkillCosts).
                Where(t => t <= currentMana).ToList();
            if (availableSkills.Count == 0) return 1;
            return 0;
        }
    }

}
