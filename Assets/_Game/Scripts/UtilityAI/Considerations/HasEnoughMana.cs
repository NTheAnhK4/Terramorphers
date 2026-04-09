using System;
using Terramorphers;
using UnityEngine;
using UtilityAI.ActionDataBuilder;

namespace UtilityAI.Consideration
{
    [CreateAssetMenu(menuName = "UtilityAI/Consideration/HasEnoughMana", fileName = "HasEnoughMana")]
    public class HasEnoughMana : Consideration
    {
        [Serializable]
        public class Data
        {
            public int SkillID;
        }
       
        [SerializeField] private Data data;
       
        public override float Evaluate(Context context, ActionExecutionData executionData)
        {
            SkillManager skillManager = context.Entity.SkillManager;
            var skillMetadata = skillManager.GetSkillMetadata(data.SkillID);
            int remainMana = context.GetData<int>(BlackBoardConstant.REMAIN_MANA_KEY);
            
            if (skillMetadata.SkillCosts <= remainMana) return 1;
            return 0;
        }
    }
}