using System;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UtilityAI.State;

namespace UtilityAI.AIActions
{
    //may be more and then use skill
    [CreateAssetMenu(menuName = "UtilityAI/AIActions/UseSkillAction", fileName = "UseSkillAction")]
    public class UseSkillAction : AIAction
    {
        
        public override async UniTask Execute(Context context)
        {
            try
            {
                await UniTask.Delay(200, cancellationToken: context.Entity.GetCancellationTokenOnDestroy());
                if (executionData.MoveTile != null)
                {
                    var moveState = context.Entity.MoveState;
                    context.Entity.ChangeState(moveState, () => new MoveStateData(){TargetTile = executionData.MoveTile});
                    await moveState.Execute(context);
                }
                
                var useSkillState = context.Entity.UseSkillState;
                context.Entity.ChangeState(useSkillState, 
                    () => new UseSkillStateData()
                    {
                        SkillID = executionData.SkillID, 
                        TargetTile = executionData.ApplySkillTile
                    });
                await useSkillState.Execute(context);
                var thinkingState = context.Entity.ThinkingState;
                await UniTask.Delay(500, cancellationToken: context.Entity.GetCancellationTokenOnDestroy());
                context.Entity.ChangeState(thinkingState);
            }
            catch(OperationCanceledException){}

        }
    }
}