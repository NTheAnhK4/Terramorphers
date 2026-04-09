using System;
using System.Collections.Generic;

using CoreGame;

using Terramorphers;



namespace UtilityAI.State
{
    
    public class ThinkingState : EnemyState<StateData>
    {
        private Dictionary<ETileType, Considerations.Consideration> tileConsiderationDict = new();
       
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
            foreach (var tileConsideration in enemyMetadata.TileConsiderationDatas)
            {
                if(tileConsideration.Consideration == null) continue;
                tileConsiderationDict[tileConsideration.TileType] = tileConsideration.Consideration;
            }
        }

        private float EvaluateTile(ITile tile)
        {
            if (tileConsiderationDict.ContainsKey(tile.TileMetadata.Type))
            {
                return tileConsiderationDict[tile.TileMetadata.Type].Evaluate(tile.Context);
            }

            return .25f;
        }
        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.UpdateContext();
            int remainStamina = entity.context.GetData<int>(BlackBoardConstant.REMAIN_STAMINA_KEY);

            List<(ITile,int)> movableTile = entity.BoardManager.GetMovableTiles(entity.CurrentTile, remainStamina);
            
            entity.context.SetData(BlackBoardConstant.TILES_EVALUATION_KEY, EvaluateTiles(movableTile));
            
            // #if UNITY_EDITOR
            // foreach (var action in entity.AIActions)
            // {
            //     entity.ActionEvaluationDebug[action.GetType().Name] = action.CaculateUtility(entity.context);
            // }
            // #endif
            // AIAction bestAction = entity.AIActions.MaxBy(t => t.CaculateUtility(entity.context));
            // if (bestAction != null) bestAction.Execute(entity.context);
            // else
            // {
            //   
            //     entity.Publisher.PublishAsync(new EndEntityTurnCommand());
            // }
        }

        private Dictionary<ITile, float> EvaluateTiles(List<(ITile, int)> tiles)
        {
            Dictionary<ITile, float> result = new();
            int remainStamina = entity.context.GetData<int>(BlackBoardConstant.REMAIN_STAMINA_KEY);
            foreach (var tile in tiles)
            {
                float pathRatio = remainStamina == 0 ? 0 : (1 - 1.0f * tile.Item2 / remainStamina);
                tile.Item1.Context.SetData(BlackBoardConstant.PATH_FROM_OWNER_TO_TILE_RATIO_KEY,pathRatio);

                float commonValue = entity.EnemyMetadata.CommonTileConsideration.Evaluate(tile.Item1.Context);
                result[tile.Item1] = commonValue * EvaluateTile(tile.Item1);
            }

            return result;
        }

       
    }
}