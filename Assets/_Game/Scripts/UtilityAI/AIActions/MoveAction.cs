using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Utility;
using Terramorphers;
using UnityEngine;
using UtilityAI.Considerations;
using UtilityAI.State;

namespace UtilityAI.AIActions
{
    public class MoveAction : AIAction
    {
        private ITile bestTile;
        public MoveAction(Enemy entity,int considerationID) : base(entity,considerationID)
        {
        }
    
        protected override void SetData(ConsiderationSystem system, ConsiderationContext context)
        {
         
            base.SetData(system, context);
            var selfContext = context.Get(EContextType.Self);
           
            var target = selfContext.GetData<TerramorphersEntity>(BlackBoardConstant.TARGET_ENTITY_KEY);
            
            context.SetParams(EContextType.Target, target.Name);
            context.SetParams(EContextType.Self, entity.Name);
        }

        public override float GetBestOption(ConsiderationSystem system, ConsiderationContext context)
        {
            Dictionary<ITile, float> tileEvaluation = context.Get(EContextType.Self)
                .GetData<Dictionary<ITile, float>>(BlackBoardConstant.TILES_EVALUATION_KEY);
            bestTile = null;
            float maxScore = float.MinValue;
         
            foreach (var item in tileEvaluation)
            {
                if (item.Value > maxScore)
                {
                    maxScore = item.Value;
                    bestTile = item.Key;
                }
            }

            return maxScore;
        }

        public override async UniTask Execute(Context context)
        {
            if (bestTile != null)
            {
                try
                {
                    #if UNITY_EDITOR
                    entity.DataDebugger["target_tile"] = bestTile;
                    #endif
                   
                    entity.ChangeState(entity.MoveState, () => new MoveStateData(){TargetTile = bestTile});
                    await entity.MoveState.Execute(context);
                    await UniTask.Delay(200, cancellationToken: entity.GetCancellationTokenOnDestroy());
                    entity.ChangeState(entity.ThinkingState);
                }
                catch(OperationCanceledException){}
            }
            
        }
    }
}