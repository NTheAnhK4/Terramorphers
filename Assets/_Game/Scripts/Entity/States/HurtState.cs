using System;
using CoreGame;
using GameCore.Domain.Skill;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Terramorphers.States
{
    public class HurtStateData : StateData
    {
        public int Damage { get; set; }
        public EAttackType AttackType { get; set; }
    }
    public class HurtState : State<TerramorphersEntity>
    {
        public HurtState(TerramorphersEntity entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public HurtState(TerramorphersEntity entity, int animationHash) : base(entity, animationHash)
        {
        }

        public HurtState(TerramorphersEntity entity) : base(entity)
        {
        }

        public HurtState(TerramorphersEntity entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            if (stateData == null || stateData is not HurtStateData hurtStateData)
            {
                entity.ChangeState(entity.IdleState);
                return;
            }

            int damage = GetDamage(hurtStateData.Damage, hurtStateData.AttackType);
           
            entity.CurrentHp -= damage;
            if (entity.CurrentHp <= 0)
            {
                entity.ChangeState(entity.DeadState);
                return;
            }
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            entity.ChangeState(entity.IdleState);
        }

        private int GetDamage(int damage, EAttackType attackType)
        {
            float remainRatio = 1;
            switch (attackType)
            {
                case EAttackType.PhysicalDamage:
                    remainRatio = Mathf.Clamp01(1 - 1.0f * entity.StatsSystem.Stats.PhysicalDamage / 100);
                    break;
                case EAttackType.MagicalDamage:
                    remainRatio = Mathf.Clamp01(1 - 1.0f * entity.StatsSystem.Stats.MagicalResistance / 100);
                    break;
                case EAttackType.NeutralDamage:
                    remainRatio = Mathf.Clamp01(1 - 1.0f * entity.StatsSystem.Stats.NeutralResistance / 100);
                    break;
            }

            return Mathf.RoundToInt(damage * remainRatio);
        }
    }
}