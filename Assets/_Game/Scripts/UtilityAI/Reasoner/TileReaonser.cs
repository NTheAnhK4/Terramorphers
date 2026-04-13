using System.Collections.Generic;
using GameCore.Domain.Tile;
using GameCore.Utility;
using Terramorphers;
using UnityEngine;
using UtilityAI.Considerations;

namespace UtilityAI.Reasoner
{
    public class TileReaonser
    {

        private Enemy entity;
        private EnemyMetadata enemyMetadata;
        private Context selfContext;
        private Dictionary<ETileType,int> tileConsiderationDict = new();

        public TileReaonser(Enemy entity)
        {
            this.entity = entity;
            this.enemyMetadata = this.entity.EnemyMetadata;
            selfContext = entity.Context;
            foreach (var tileConsideration in enemyMetadata.TileConsiderationDatas)
            {
                if(tileConsideration.ConsiderationID < 0) continue;
                tileConsiderationDict[tileConsideration.TileType] = tileConsideration.ConsiderationID;
            }
        }

        public void EvaluateTile(ConsiderationSystem system, ConsiderationContext context, TerramorphersEntity target)
        {
            int remainStamina = entity.Context.GetData<int>(BlackBoardConstant.REMAIN_STAMINA_KEY);

            var movableTiles = entity.BoardManager.GetMovableTiles(entity.CurrentTile, remainStamina);
            int maxDisToTarget = 0;
            foreach (var item in movableTiles)
            {
                maxDisToTarget = Mathf.Max(maxDisToTarget, item.Item1.Context.GetData<int>(string.Format(BlackBoardConstant.ENTITY_TO_TILE_DISTANCE_KEY, target.Name)));
            }

            foreach (var item in movableTiles)
            {
                item.Item1.Context.SetData(
                    string.Format(BlackBoardConstant.ENTITY_NEARNESS_RATIO, target.Name),
                   1 - item.Item1.Context.GetData<int>(string.Format(BlackBoardConstant.ENTITY_TO_TILE_DISTANCE_KEY, target.Name)) * 1.0f / maxDisToTarget);

            }
            
          
            context.SetParams(EContextType.Target, target.Name);
            entity.Context.SetData(BlackBoardConstant.TILES_EVALUATION_KEY, EvaluateTiles(context, movableTiles));
        }
        private Dictionary<ITile, float> EvaluateTiles(ConsiderationContext considerationContext,List<(ITile,int)> movableTiles)
        {
            Dictionary<ITile, float> result = new();
          
            foreach (var item in movableTiles)
            {
                result[item.Item1] = EvaluateTile(considerationContext, item.Item1);

            }

           

            return result;
        }
        private float EvaluateTile(ConsiderationContext considerationContext, ITile tile)
        {
            considerationContext.Set(EContextType.Tile, tile.Context);
            float commonEvaluation = enemyMetadata.ConsiderationSystem.Evaluate(enemyMetadata.CommonTileConsiderationID, considerationContext);
            if (tileConsiderationDict.TryGetValue(tile.TileMetadata.Type, out var considerationID))
            {
                return enemyMetadata.ConsiderationSystem.Evaluate(considerationID, considerationContext) * commonEvaluation;
               
            }

            return commonEvaluation;
        }
    }

}