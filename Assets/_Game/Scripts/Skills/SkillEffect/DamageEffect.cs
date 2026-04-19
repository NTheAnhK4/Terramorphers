using GameCore.Domain.Skill;
using GameCore.Domain.Stats;
using System;
using UnityEngine;

namespace Terramorphers.Skill
{
    [Serializable]
    public class DamageEffect : IEffect
    {
        [SerializeField] private EEffectTriggerType triggerType;
        [SerializeField] private int damage;
        public void Execute<T>(T owner, EEffectTriggerType trigger)
        {
            if(trigger != triggerType) return;
            if (owner is not TerramorphersEntity entity) return;
            entity.TakeDamage(damage, EAttackType.NeutralDamage);
        }
    }
}