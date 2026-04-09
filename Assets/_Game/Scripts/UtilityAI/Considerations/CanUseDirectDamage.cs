using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Utility.Shape;
using Terramorphers;
using Terramorphers.Stats;
using UnityEngine;
using UtilityAI.ActionDataBuilder;


namespace UtilityAI.Consideration
{
    [CreateAssetMenu(menuName = "UtilityAI/Consideration/CanUseDirectDamage", fileName = "CanUseDirectDamage")]
    public class CanUseDirectDamage : Consideration
    {
        [Serializable]
        public class Data
        {
            public int SkillID;
            public AnimationCurve Curve;
        }

        [SerializeField] private Data data;
        
        public override float Evaluate(Context context, ActionExecutionData executionData)
        {
            SkillManager skillManager = context.Entity.SkillManager;
            var skillMetadata = skillManager.GetSkillMetadata(data.SkillID);
            if (skillMetadata.Range == 0)
            {
                //only apply for self
                return 0;
            }

            StatsSystem statsSystem = context.Entity.StatsSystem;

            int teamID = context.Entity.TeamID;
            ITile currentTile = context.Entity.CurrentTile;
            var closestEnemy = context.Entity.EntityManager.GetClosestEntityWithDiffTeamID(teamID, currentTile);
            if (closestEnemy == null) return 0;
            int range = statsSystem.Stats.Range + skillMetadata.Range;
            //can attack immidiately
            if (context.Entity.BoardManager.HexaBoard.IsCubeVisible(currentTile.Index, closestEnemy.CurrentTile.Index, t => t.IsBlockVisibility()))
            {
                float distance = Cube.Distance(currentTile.Index, closestEnemy.CurrentTile.Index);
               
                if (distance <= range)
                {
                   
                    context.CandidateMoveTile = null;
                    context.CandidateSkillID = data.SkillID;
                    context.CandidateTileApply = closestEnemy.CurrentTile;
                    return 1;
                }
              
            }

            int remainStamina = context.GetData<int>(BlackBoardConstant.REMAIN_STAMINA_KEY);
            if (remainStamina <= 0)
            {
              
                return 0;
            }

            List<(ITile, int)> movableTile = context.Entity.BoardManager.HexaBoard.
                GetMovableAndDistanceValue(currentTile.Index,
                    remainStamina, 
                    t => t.GetMoveCost(), 
                    t => !t.IsPassable()).ToList();
            if (movableTile.Count == 0)
            {
               
                return 0;
            }
            
            
            int minDistance = int.MaxValue;
            ITile closestTile = null;
            foreach (var tileDistance in movableTile)
            {
                bool isVisible = context.Entity.BoardManager.HexaBoard.IsCubeVisible(tileDistance.Item1.Index, closestEnemy.CurrentTile.Index, t => t.IsBlockVisibility());
                float distance = Cube.Distance(tileDistance.Item1.Index, closestEnemy.CurrentTile.Index);
                if (isVisible && distance <= range)
                {
                    if (tileDistance.Item2 < minDistance)
                    {
                        minDistance = tileDistance.Item2;
                        closestTile = tileDistance.Item1;
                    }
                }
            }

            if (closestTile == null)
            {
               
                return 0;
            }
            context.CandidateMoveTile = closestTile;
            context.CandidateSkillID = data.SkillID;
            context.CandidateTileApply = closestEnemy.CurrentTile;
            float raw = data.Curve.Evaluate( 1.0f * minDistance / remainStamina);
            return Mathf.Clamp01(raw);
        }
        private void OnValidate()
        {
            data = new Data();
            data.Curve = new AnimationCurve(
                new Keyframe(0f, 1f),
                new Keyframe(1f, 0f)
            );
        }
    }
}