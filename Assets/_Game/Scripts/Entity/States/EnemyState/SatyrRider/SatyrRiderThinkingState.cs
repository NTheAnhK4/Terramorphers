using System;
using System.Collections.Generic;
using CoreGame;
using GameCore.Commands;
using GameCore.Utility.Shape;
using UnityEngine;

namespace Terramorphers
{
    public class SatyrRiderThinkingState : State<SatyrRider>
    {
     
        public SatyrRiderThinkingState(SatyrRider entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public SatyrRiderThinkingState(SatyrRider entity) : base(entity)
        {
        }

        public SatyrRiderThinkingState(SatyrRider entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }
        public SatyrRiderThinkingState(SatyrRider entity, int animHash) : base(entity, animHash){}

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.ResetMinStateCost();
            
            List<TerramorphersEntity> terramorphersEntities = entity.EntityManager.GetEnemies(entity);
            if (terramorphersEntities == null || terramorphersEntities.Count == 0)
            {
                
                return;
            }

            var nearestEnemy = GetNearestEnemy(terramorphersEntities);
            if (nearestEnemy == null) return;
            float distance = Cube.Distance(entity.CurrentTile.Index, nearestEnemy.CurrentTile.Index);
            if (distance <= entity.AttackRange)
            {
               
                entity.Publisher.PublishAsync(new EndEntityTurnCommand());
            }
            else
            {
                
                entity.MovePath = entity.BoardManager.GetPathWithLimitDistance(entity.CurrentTile, nearestEnemy.CurrentTile, entity.RemainStamina);
                if(entity.MovePath[^1] != nearestEnemy.CurrentTile) entity.MovePath.Add(nearestEnemy.CurrentTile);
             
                if (entity.MovePath == null || entity.MovePath.Count < 2)
                {
                    
                    entity.Publisher.PublishAsync(new EndEntityTurnCommand());
                }
                else
                {
                    entity.SetStateWithMinCost(entity.MoveState);
                    entity.CaculateMinCost();
                   
                }
            }

        }

      

       

        private TerramorphersEntity GetNearestEnemy(List<TerramorphersEntity> enemies)
        {
            Cube index = entity.CurrentTile.Index;
            float minDistance = float.MaxValue;
            TerramorphersEntity result = null;
            foreach (var enemy in enemies)
            {
                Cube enemyIndex = enemy.CurrentTile.Index;
                float distance = Cube.Distance(index, enemyIndex);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    result = enemy;
                }
            }

            return result;
        }
        
        
    }
}

