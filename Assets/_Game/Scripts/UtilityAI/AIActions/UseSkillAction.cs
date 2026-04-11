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
      

        private Dictionary<int, ThinkingState.SkillInfo> skillInfos = new();
        private EnemyMetadata enemyMetadata;
        private List<SkillMetadata> skillMetadatas = new();
        private int bestSkillID;
        public UseSkillAction(int considerationID) : base(considerationID)
        {
        }

        public UseSkillAction(int considerationID, Dictionary<int, ThinkingState.SkillInfo> skillInfos) : base(considerationID)
        {
            this.skillInfos = skillInfos;
        }


        public override float GetBestOption(ConsiderationSystem system, ConsiderationContext context)
        {
            float maxScore = float.MinValue;
            foreach (var skillItem in skillInfos)
            {
                context.Set(EContextType.Tile, skillItem.Value.Context);
                float score = system.Evaluate(skillItem.Key, context);
                if (score > maxScore)
                {
                    maxScore = score;
                    bestSkillID = skillItem.Key;
                }
            }

            return maxScore;
        }

      

        public override UniTask Execute(Context context)
        {
            Debug.Log($"[Test] i will use skill {bestSkillID}");
            return UniTask.CompletedTask;
        }
    }
}