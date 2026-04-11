using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Utility;
using UtilityAI.Considerations;
using VitalRouter;

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
            var selfContext = entity.Context;
            int remainStamina = selfContext.GetData<int>(BlackBoardConstant.REMAIN_STAMINA_KEY);
            int maxStamina = entity.StatsSystem.Stats.Stamina;
            float staminaAvailability;
            if (maxStamina == 0) staminaAvailability = 0;
            else staminaAvailability = 1.0f * remainStamina / maxStamina;
            selfContext.SetData(BlackBoardConstant.STAMINA_AVAILABILITY_RATIO, staminaAvailability);
            
            int remainMana = entity.Context.GetData<int>(BlackBoardConstant.REMAIN_MANA_KEY);
            entity.Context.SetData(BlackBoardConstant.MANA_AVAILABILITY_RATIO,1.0f * remainMana/entity.StatsSystem.Stats.Mana);
            context.SetParams(EContextType.Self, entity.Name);
        }

        public override float GetBestOption(ConsiderationSystem system, ConsiderationContext context) => 1;

        public override UniTask Execute(Context context)
        {
            entity.Publisher.PublishAsync(new EndEntityTurnCommand() { });
            return UniTask.CompletedTask;
        }
    }
}