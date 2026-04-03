
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using UnityEngine;


namespace Terramorphers.Skill
{
    public class AttackSkillHandler : SkillHandler<ITile>
    {
        [SerializeField] private float delayTime;
        [SerializeField] private int damage;
        [SerializeField] private EAttackType attackType;


        protected override async UniTask Use(ITile context, CancellationToken token)
        {
            if (context == null)
            {
                Debug.Log($"[Test] context used in attackSkillHandler is null");
                return;
            }
            try
            {
                if (delayTime > 0) await UniTask.Delay(TimeSpan.FromSeconds(delayTime), cancellationToken: token);
                TerramorphersEntity entity = context.CurrentOccupant;
                if (entity == null)
                {
                    Debug.Log($"[Test] attack skill handler required entity");
                    return;
                }
                entity.TakeDamage(damage, attackType);
                
            }catch(OperationCanceledException){}
        }
    }
}

