
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using GameCore.Utility;
using JSAM;
using UnityEngine;


namespace Terramorphers.Skill
{
    [Serializable]
    public class AttackSkillHandler : SkillHandler<TerramorphersEntity,ITile>
    {

      
        [SerializeField] private int damage;
        [SerializeField] private EAttackType attackType;
        [SerializeField] private SkillEffectData skillEffectData;

     

        protected override async UniTask Use(TerramorphersEntity owner, ITile target, CancellationToken token)
        {
            try
            {
                if (delayTime > 0) await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: token);
              
                TerramorphersEntity entityTarget = target.CurrentOccupant;
                if (entityTarget == null || owner == null) return;
                entityTarget.TakeDamage(owner.StatsSystem.Stats.GetDamage(damage, attackType), attackType);
                if (skillEffectData.EffectType != ESkillEffectType.None)
                {
                    if(skillEffectData.TargetType == SkillEffectData.EEffectTargetType.Self) owner.StatsSystem.AddModifier(skillEffectData.GetStatModifier());
                    else entityTarget.StatsSystem.AddModifier(skillEffectData.GetStatModifier());
                }
            }
            catch(OperationCanceledException){}
        }

        public override void SetUpContext(Context context)
        {
            
        }
    }
}

