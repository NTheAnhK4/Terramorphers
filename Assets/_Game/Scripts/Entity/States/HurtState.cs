
using CoreGame;
using GameCore.Domain.Quest;
using GameCore.Domain.Skill;
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
        

        public HurtState(TerramorphersEntity entity, int animationHash) : base(entity, animationHash)
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

            int damage = entity.StatsSystem.Stats.GetDamageTaken(hurtStateData.Damage, hurtStateData.AttackType);
            
            
            if (entity is Player)
            {
                entity.QuestUseCase.IncreaseQuestProgress(
                    EQuestActionType.Limit,
                    EQuestTargetType.DamageTaken,
                    0, 
                    damage);
            }
            entity.DataCache.RemainHP.Value -= damage;
           
            if (entity.DataCache.RemainHP.Value <= 0)
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

       
    }
}