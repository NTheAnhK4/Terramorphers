using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Commands;
using Terramorphers.Command;
using UnityEngine;
using VitalRouter;

namespace Terramorphers.States.PlayerState
{
    public class PlayerMoveState : State<Player>
    {
       
        public PlayerMoveState(Player entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public PlayerMoveState(Player entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.Publisher.PublishAsync(new EnableEndTurnCommand() { IsEnable = false });
            entity.Publisher.PublishAsync(new EnableSkillCommand() { IsEnable = false });
            entity.Publisher.PublishAsync(new ClearSpecialTilesCommand());

            MoveToTargetTile();

        }

       

        private void MoveToTargetTile()
        {
            var moveTiles = entity.BoardManager.GetPath(entity.CurrentTile, entity.SelectedTile);
            if (moveTiles == null || moveTiles.Count <= 1) return;
            var movePath = moveTiles.Select(t => t.Transform.position).ToArray();
            entity.transform.DOPath(movePath,(movePath.Count() - 1) * entity.MoveSpeed, PathType.Linear).OnWaypointChange(index =>
            {
                entity.SetTile(moveTiles[index]);
            }).OnComplete(() =>
            {
              
                entity.RemainStamina -= moveTiles.Skip(1).Select(t => t.GetMoveCost()).Sum();
             
                entity.ChangeState(entity.SelectMoveTileState);
            });
        }
       
    }
}