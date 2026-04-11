using System;
using System.Collections.Generic;

using CoreGame;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;

using GameCore.Utility;
using Terramorphers;
using UnityEngine;
using UtilityAI.AIActions;
using UtilityAI.Considerations;
using UtilityAI.Reasoner;


namespace UtilityAI.State
{
    
    public class ThinkingState : EnemyState<StateData>
    {
        public class SkillInfo
        {
            public string AnimaName;
            public int ConsiderationID;
            public Context Context;
            public SkillMetadata SkillMetaData;
        }
       
        private EnemyMetadata enemyMetadata;

        private int remainStamina;
        private List<(ITile, int)> movableTiles = new();
        private ConsiderationContext considerationContext = new();
        private List<AIAction> aiActions = new();
        Dictionary<int, SkillInfo> skillInfos = new();
        private EntityReasoner entityReasoner;
        private TileReaonser tileReasoner;
        private SkillReasoner skillReasoner;
        public ThinkingState(Enemy entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public ThinkingState(Enemy entity, int animationHash) : base(entity, animationHash)
        {
        }

        public ThinkingState(Enemy entity) : base(entity)
        {
        }

        public ThinkingState(Enemy entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }

        public ThinkingState(Enemy entity, int animationHash, EnemyMetadata enemyMetadata)
            : base(entity, animationHash)
        {
            this.enemyMetadata = enemyMetadata;
           
            considerationContext.Set(EContextType.Self, entity.Context);

            entityReasoner = new EntityReasoner(entity);
            tileReasoner = new TileReaonser(entity);
          
            SkillManager skillManager = entity.SkillManager;
            foreach (var skillconsiderationData in enemyMetadata.SkillConsiderationDatas)
            {
                var skillMetadata = skillManager.GetSkillMetadata(skillconsiderationData.SkillID);
                var context = skillMetadata.GetContext();
                skillInfos[skillconsiderationData.SkillID] = new SkillInfo()
                {
                    AnimaName = skillconsiderationData.AnimName,
                    ConsiderationID = skillconsiderationData.ConsiderationID,
                    Context = context,
                    SkillMetaData = skillMetadata
                };

            }

            skillReasoner = new SkillReasoner(entity, skillInfos);
            aiActions.Add(new MoveAction(entity, enemyMetadata.MoveActionConsiderationID));
            aiActions.Add(new UseSkillAction(entity,enemyMetadata.UseSkillActionConsiderationID,  skillInfos));
            aiActions.Add(new EndTurnAction(entity, this.enemyMetadata.EndTurnActionConsiderationID));
        }

       
        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.UpdateContext();

            var targetEntity = entityReasoner.GetBestEntity(enemyMetadata.ConsiderationSystem, considerationContext);
            if (targetEntity == null)
            {
                Debug.Log($"[Test] no entity valid");
                return;
            }
            entity.Context.SetData(BlackBoardConstant.TARGET_ENTITY_KEY, targetEntity);
            entity.DerivedDataCalculator.OnTargetChange(targetEntity);
            tileReasoner.EvaluateTile(enemyMetadata.ConsiderationSystem, considerationContext, targetEntity);
            skillReasoner.EvaluateSkill(enemyMetadata.ConsiderationSystem, considerationContext, targetEntity);

            AIAction bestAction = null;
            float highestScore = float.MinValue;
            foreach (var action in aiActions)
            {
                float score = action.CaculateUtility(enemyMetadata.ConsiderationSystem, considerationContext);
                #if UNITY_EDITOR
                entity.DataDebugger[action.GetType().Name] = score;
                #endif
              
                if (highestScore < score)
                {
                    highestScore = score;
                    bestAction = action;
                }
            }

            if (bestAction != null)
            {
                bestAction.Execute(entity.Context).Forget();
            }
        }

       
      

      

       

     

       
    }

    
}