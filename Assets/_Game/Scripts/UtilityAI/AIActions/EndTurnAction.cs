using System;
using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Utility;
using UtilityAI.Considerations;


namespace UtilityAI.AIActions
{
    public class EndTurnAction : AIAction
    {
        public EndTurnAction(Enemy entity, int considerationID) : base(entity, considerationID)
        {
        }

        protected override void SetData(ConsiderationSystem system, ConsiderationContext context)
        {
            base.SetData(system, context);
            context.SetParams(EContextType.Self, entity.Name);
        }

        public override float GetBestOption(ConsiderationSystem system, ConsiderationContext context) => 1;

        public override async UniTask Execute(Context context)
        {
            try
            {
                await UniTask.Delay(100, cancellationToken: entity.GetCancellationTokenOnDestroy());
                await entity.Publisher.PublishAsync(new EndEntityTurnCommand() { });
            }
           
            catch(OperationCanceledException){}
        }
    }
}