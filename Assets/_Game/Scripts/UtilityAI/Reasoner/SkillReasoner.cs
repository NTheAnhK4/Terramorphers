using System.Collections.Generic;
using GameCore.Utility;
using GameCore.Utility.Shape;
using Terramorphers;
using UnityEngine;
using UtilityAI.Considerations;
using UtilityAI.State;

namespace UtilityAI.Reasoner
{
    public class SkillReasoner
    {
        private EnemyMetadata enemyMetadata;
        private Enemy entity;
        private Dictionary<int, ThinkingState.SkillInfo> skillInfos;
        public SkillReasoner(Enemy entity, Dictionary<int, ThinkingState.SkillInfo> skillInfos)
        {
            this.enemyMetadata = entity.EnemyMetadata;
            this.entity = entity;
            this.skillInfos = skillInfos;
        }

        private void SetData(ConsiderationSystem system, ConsiderationContext considerationContext, TerramorphersEntity target)
        {
            foreach (var skillItem in skillInfos)
            {
                Context context = skillItem.Value.Context;
                var skillMetadata = skillItem.Value.SkillMetaData;
                
                //mana
                int remainMana = entity.Context.GetData<int>(BlackBoardConstant.REMAIN_MANA_KEY);
                float manaAffordabilityRatio = remainMana < skillMetadata.SkillCosts ? 0 :(1 - 1.0f * skillMetadata.SkillCosts / remainMana);
                context.SetData(BlackBoardConstant.MANA_AFFORDABILITY_RATIO, manaAffordabilityRatio);
                
            
               
                var hexa = entity.BoardManager.HexaBoard;
                
                //visible
               
                float visibility = 0;
                if (hexa.IsCubeVisible(target.CurrentTile.Index, 
                        entity.CurrentTile.Index, 
                        tile => tile.IsBlockVisibility()))
                {
                    visibility = 1;
                }
                context.SetData(string.Format(BlackBoardConstant.ENTITY_VISIBILITY_RATIO, target.Name), visibility);
                
                //range
                int range;
                if (skillMetadata.Range == 0) range = 1;
                else range = skillMetadata.Range + entity.StatsSystem.Stats.Range;
              
                float distance = Cube.Distance(target.CurrentTile.Index, entity.CurrentTile.Index);
                float rangeAvailable = Mathf.Clamp01(1 - distance / range);
                context.SetData(string.Format(BlackBoardConstant.SKILL_RANGE_AVAILABILITY_RATIO, skillMetadata.SkillName),rangeAvailable);

            }
        }
        public void EvaluateSkill(ConsiderationSystem system, ConsiderationContext considerationContext, TerramorphersEntity target)
        {
           SetData(system, considerationContext, target);
           Dictionary<int, float> skillEvaluation = new();
           foreach (var skillItem in skillInfos)
           {
               considerationContext.Set(EContextType.Skill, skillItem.Value.Context);
               considerationContext.SetParams(EContextType.Skill, skillItem.Value.SkillMetaData.SkillName);
               considerationContext.SetParams(EContextType.Target, target.Name);
               float score = system.Evaluate(skillItem.Value.ConsiderationID, considerationContext);
               skillEvaluation[skillItem.Key] = score;
           }
           entity.Context.SetData(BlackBoardConstant.SKILL_EVALUATION_KEY, skillEvaluation);
        }
    }
}