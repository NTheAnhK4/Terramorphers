using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Commands;
using UnityEngine;

namespace Terramorphers.States.EnemyState
{
    public class EnemyMoveState : State<Enemy>
    {
        public EnemyMoveState(Enemy entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public EnemyMoveState(Enemy entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }

        public EnemyMoveState(Enemy entity, int animHash) : base(entity, animHash)
        {
            
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.ResetMinStateCost();
            MoveToTargetTile();


        }
        private void MoveToTargetTile()
        {
            var moveTiles = entity.MovePath;
            if (moveTiles == null || moveTiles.Count < 2)
            {
                entity.TurnToThinkingState();
             
                return;
            }

            var movePath = moveTiles.SkipLast(1).Select(t => t.Transform.position).ToArray();
            List<Vector3> directionList = new();
            for(int i = 0; i < moveTiles.Count - 1; ++i)
            {
                Vector3 direction = moveTiles[i + 1].Transform.position - moveTiles[i].Transform.position;
                directionList.Add(direction);
            }

          
            entity.transform.DOPath(movePath,movePath.Count() * entity.MoveSpeed, PathType.Linear)
             
                .OnWaypointChange(index =>
            {
                

                if (index < directionList.Count)
                    entity.SetDirection(directionList[index]);
            }).OnComplete(() =>
            {
                entity.RemainStamina -= moveTiles.Skip(1).Select(t => t.GetMoveCost()).Sum();
                if (moveTiles.Count >= 2)
                    entity.SetTile(moveTiles[^2]);

                if (directionList.Count > 0)
                    entity.SetDirection(directionList.Last());
                entity.TurnToThinkingState();

            });
        }

        
    }
}