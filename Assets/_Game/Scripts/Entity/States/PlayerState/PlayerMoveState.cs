using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Commands;
using GameCore.Utility.Audio.GameAudio;
using JSAM;
using Terramorphers.Command;
using UnityEngine;
using VitalRouter;

namespace Terramorphers.States.PlayerState
{
    public class PlayerMoveStateData : StateData
    {
        public ITile TargetTile { get; set; }
    }
    public class PlayerMoveState : State<Player>
    {
        private PlayerMoveStateData data;
       
        public PlayerMoveState(Player entity, int animHash) : base(entity, animHash){}

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            if (stateData == null || stateData is not PlayerMoveStateData playerMoveStateData)
            {
                entity.ChangeState(entity.PlayerSelectMoveTileState);
                return;
            }

            AudioManager.PlaySound(ESoundType.FootstepDirt);

            data = playerMoveStateData;
            entity.Publisher.PublishAsync(new EnableEndTurnCommand() { IsEnable = false });
            entity.Publisher.PublishAsync(new EnableSkillCommand() { IsEnable = false });
            entity.Publisher.PublishAsync(new ClearSpecialTilesCommand());

            MoveToTargetTile();

        }

       

        private void MoveToTargetTile()
        {
            var moveTiles = entity.BoardManager.GetPath(entity.CurrentTile, data.TargetTile);
            if (moveTiles == null || moveTiles.Count <= 1) return;
            var movePath = moveTiles.Select(t => t.Transform.position).ToArray();
            List<Vector3> directionList = new();
            for(int i = 0; i < moveTiles.Count - 1; ++i)
            {
                Vector3 direction = moveTiles[i + 1].Transform.position - moveTiles[i].Transform.position;
                directionList.Add(direction);
            }
            entity.transform.DOPath(movePath,(movePath.Count() - 1) * entity.MoveSpeed, PathType.Linear).OnWaypointChange(index =>
            {
                if (index < directionList.Count)
                    entity.SetDirection(directionList[index]);
                if (index > 0 && index < moveTiles.Count()) entity.DataCache.RemainStamina.Value -= moveTiles[index].GetMoveCost();
                entity.SetTile(moveTiles[index]);
            }).OnComplete(() =>
            {
              
                //entity.RemainStamina -= moveTiles.Skip(1).Select(t => t.GetMoveCost()).Sum();
             
                entity.ChangeState(entity.PlayerSelectMoveTileState);
            });
        }

        public override void OnExit()
        {
            base.OnExit();
            AudioManager.StopSound(ESoundType.FootstepDirt);
        }
    }
}