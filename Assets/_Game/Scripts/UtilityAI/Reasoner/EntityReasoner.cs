using System.Collections.Generic;
using GameCore.Utility;
using Terramorphers;
using UnityEngine;
using UtilityAI.Considerations;

namespace UtilityAI.Reasoner
{
    public class EntityReasoner
    {
       
        private Enemy entity;
        private EnemyMetadata enemyMetadata;
        private Context selfContext;
        public EntityReasoner(Enemy entity)
        {
            this.entity = entity;
            this.enemyMetadata = this.entity.EnemyMetadata;
            selfContext = entity.Context;
        }

        private void SetData(ConsiderationSystem system,ConsiderationContext considerationContext)
        {
           
        }
        

        public TerramorphersEntity GetBestEntity(ConsiderationSystem system,ConsiderationContext considerationContext)
        {
            SetData(system, considerationContext);
            float maxScore = float.MinValue;
            TerramorphersEntity bestEntity = null;
            if (enemyMetadata.AllyConsiderationID >= 0)
            {
                List<TerramorphersEntity> allies = entity.EntityManager.GetEntitiesWithTeamID(entity.TeamID);
                allies.Remove(entity);
                Dictionary<TerramorphersEntity, float> allyEvaluation = new();
                foreach (var ally in allies)
                {
                    considerationContext.Set(EContextType.Ally, ally.Context);
                    considerationContext.SetParams(EContextType.Ally, ally.Name);
                    float score = system.Evaluate(enemyMetadata.AllyConsiderationID, considerationContext);
                    allyEvaluation[ally] = score;
                    if (score > maxScore)
                    {
                        bestEntity = ally;
                        maxScore = score;
                    }
                }
                selfContext.SetData(BlackBoardConstant.ALLY_REASONER, allyEvaluation);
            }

            if (enemyMetadata.EnemyConsiderationID >= 0)
            {
                List<TerramorphersEntity> enemies =  entity.EntityManager.GetEntitiesWithDiffTeamID(entity.TeamID);
                Dictionary<TerramorphersEntity, float> enemyEvaluation = new();
                foreach (var enemy in enemies)
                {
                    considerationContext.Set(EContextType.Enemy, enemy.Context);
                    considerationContext.SetParams(EContextType.Enemy, enemy.Name);
                    float score = system.Evaluate(enemyMetadata.EnemyConsiderationID, considerationContext);
                    enemyEvaluation[enemy] = score;
                    if (score > maxScore)
                    {
                        bestEntity = enemy;
                        maxScore = score;
                    }
                }
                selfContext.SetData(BlackBoardConstant.ENEMY_REASONER, enemyEvaluation);
                
            }
            if(enemyMetadata.SelfConsiderationID  >= 0)
            {
                considerationContext.SetParams(EContextType.Self, entity.Name);
                float score = system.Evaluate(enemyMetadata.SelfConsiderationID, considerationContext);
                selfContext.SetData(BlackBoardConstant.SELF_REASONER, score);
                if (score > maxScore)
                {
                    bestEntity = entity;
                    maxScore = score;
                }
            }

            return bestEntity;
        }
        
    }
}