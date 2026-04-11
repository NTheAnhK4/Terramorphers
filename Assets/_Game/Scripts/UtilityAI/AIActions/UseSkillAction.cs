using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using GameCore.Utility;
using Terramorphers;
using UnityEngine;
using UtilityAI.Considerations;
using UtilityAI.State;

namespace UtilityAI.AIActions
{
    public class UseSkillAction : AIAction
    {


        private Dictionary<int, ThinkingState.SkillInfo> skillInfos;
        private EnemyMetadata enemyMetadata;
       
        private int bestSkillID;
        
        public UseSkillAction(Enemy entity,int considerationID, Dictionary<int, ThinkingState.SkillInfo> skillInfos) : base(entity,considerationID)
        {
            this.skillInfos = skillInfos;
        }


        public override float GetBestOption(ConsiderationSystem system, ConsiderationContext context)
        {
            Dictionary<int, float> skillEvaluation = context.Get(EContextType.Self)
                .GetData<Dictionary<int, float>>(BlackBoardConstant.SKILL_EVALUATION_KEY);
            float maxScore = float.MinValue;
            foreach (var item in skillEvaluation)
            {
                if (item.Value > maxScore)
                {
                    maxScore = item.Value;
                    bestSkillID = item.Key;
                }
            }
            return maxScore;
        }

      

        public override UniTask Execute(Context context)
        {
            #if UNITY_EDITOR
            entity.DataDebugger["skillID"] = bestSkillID;
            #endif
            Debug.Log($"[Test] i will use skill {bestSkillID}");
            return UniTask.CompletedTask;
        }
    }
}