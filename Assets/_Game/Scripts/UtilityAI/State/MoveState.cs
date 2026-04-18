using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Terramorphers;
using UnityEngine;
using GameCore.Utility;
using GameCore.Utility.Audio.GameAudio;
using JSAM;

namespace UtilityAI.State
{
    public class MoveStateData : StateData
    {
        public ITile TargetTile;
    }
    public class MoveState : EnemyState<MoveStateData>
    {
       

        public override async UniTask Execute(Context context)
        {
            base.Execute(context);
            try
            {
                AudioManager.PlaySound(ESoundType.FootstepDirt);
                ITile currentTile = entity.CurrentTile;

                List<ITile> moveTiles = entity.BoardManager.GetPath(currentTile, data.TargetTile);
                if (moveTiles == null || moveTiles.Count <= 1) return;
                var movePath = moveTiles.Select(t => t.Transform.position).ToArray();
                List<Vector3> directionList = new();
                for (int i = 0; i < moveTiles.Count - 1; ++i)
                {
                    Vector3 direction = moveTiles[i + 1].Transform.position - moveTiles[i].Transform.position;
                    directionList.Add(direction);
                }

                var tween = entity.transform.DOPath(movePath, (movePath.Count() - 1) * .75f).OnWaypointChange(index =>
                {
                    if (index < directionList.Count)
                        entity.SetDirection(directionList[index]);
                    // if (index > 0 && index < moveTiles.Count())
                    // {
                    //     entity.DataCache.RemainStamina.Value -= moveTiles[index].GetMoveCost();
                    // }

                    entity.SetTile(moveTiles[index]);
                });
                await tween.AwaitForStepComplete(cancellationToken: entity.transform.GetCancellationTokenOnDestroy());
                entity.DataCache.RemainStamina.Value -= moveTiles.Select(t => t.GetMoveCost()).Sum();
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                AudioManager.StopSound(ESoundType.FootstepDirt);
            }

        }

      
        public MoveState(Enemy entity, int animationHash) : base(entity, animationHash)
        {
        }

      
    }
}