using System;
using CoreGame;
using GameCore.Utility;
using UtilityAI.AIActions;

using GameCore.Commands;
using UnityEngine;


namespace UtilityAI.State
{
    
    public class ThinkingState : EnemyState<StateData>
    {
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

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.UpdateContext();
            
            foreach(var action in entity.AIActions) action.Build();
            
            #if UNITY_EDITOR
            foreach (var action in entity.AIActions)
            {
                entity.ActionEvaluationDebug[action.GetType().Name] = action.CaculateUtility(entity.context);
            }
            #endif
            AIAction bestAction = entity.AIActions.MaxBy(t => t.CaculateUtility(entity.context));
            if (bestAction != null) bestAction.Execute(entity.context);
            else
            {
              
                entity.Publisher.PublishAsync(new EndEntityTurnCommand());
            }
        }
    }
}