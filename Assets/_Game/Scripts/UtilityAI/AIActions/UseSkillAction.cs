using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using GameCore.Utility;
using Terramorphers;

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

      

        public override async UniTask Execute(Context context)
        {
            #if UNITY_EDITOR
            entity.DataDebugger["skillID"] = bestSkillID;
            #endif
            try
            {
                var target = context.GetData<TerramorphersEntity>(BlackBoardConstant.TARGET_ENTITY_KEY);
                entity.ChangeState(entity.UseSkillState, () => new UseSkillStateData()
                {
                    SkillID = bestSkillID,
                    TargetTile = target.CurrentTile
                });
                await entity.UseSkillState.Execute(context);
                await UniTask.Delay(100, cancellationToken: entity.GetCancellationTokenOnDestroy());
                entity.ChangeState(entity.ThinkingState);
            }
            catch(OperationCanceledException){}
           
        }
    }
}