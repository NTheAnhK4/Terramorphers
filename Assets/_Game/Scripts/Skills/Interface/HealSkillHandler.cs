
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using GameCore.Utility;
using UnityEngine;

namespace Terramorphers.Skill
{
    [Serializable]
    public class HealSkillHandler : SkillHandler<TerramorphersEntity,ITile>
    {
        [SerializeField] private float delayTime;
        [SerializeField] private int healthAmount;
        protected override async UniTask Use(TerramorphersEntity owner, ITile target, CancellationToken token)
        {
            try
            {
                if (delayTime > 0) await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: token);
                TerramorphersEntity entityTarget = target.CurrentOccupant;
                if (entityTarget == null || owner == null) return;
                entityTarget.Heal(owner.StatsSystem.Stats.GetHealAmount(healthAmount));
            }
            catch(OperationCanceledException){}
        }

        public override void SetUpContext(Context context)
        {
            
        }
    }

}
