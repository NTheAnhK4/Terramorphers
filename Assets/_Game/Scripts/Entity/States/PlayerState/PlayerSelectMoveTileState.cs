using System;
using System.Collections.Generic;
using CoreGame;
using GameCore.Commands;
using Terramorphers.Command;
using UnityEngine;
using VitalRouter;

namespace Terramorphers.States.PlayerState
{
    public class PlayerSelectMoveTileState : State<Player>
    {

        private List<IDisposable> _disposables = new();
      
        public PlayerSelectMoveTileState(Player entity, int animHash) : base(entity, animHash){}

        public override void OnEnter(StateData stateData = null)
        {
          
            base.OnEnter(stateData);
            entity.InputManager.SetLayer(InputManager.TILE_LAYER);
            entity.Publisher.PublishAsync(new EnableEndTurnCommand() { IsEnable = true });
            entity.Publisher.PublishAsync(new EnableSkillCommand() { IsEnable = true });
            entity.Publisher.PublishAsync(new SetMovableTilesCommand()
            {
                CenterTile = entity.CurrentTile, 
                Distance = entity.DataCache.RemainStamina.Value
            });
            _disposables.Add(entity.Subscribable.Subscribe<SelectTileCommand>(OnSelectTile)); 
            _disposables.Add(entity.Subscribable.Subscribe<UseSkillCommand>(UseSkill));
        }

        public override void Update()
        {
            base.Update();
          
            entity.InputManager.OnUpdate();
        }

        private void UseSkill(UseSkillCommand command, PublishContext context)
        {
            PlayerSelectSkillTileData playerSelectSkillTileData = new PlayerSelectSkillTileData() { SkillID = command.SkillID };
            entity.ChangeState(entity.SelectSkillTileState, () => playerSelectSkillTileData);
        }

        private void OnSelectTile(SelectTileCommand command, PublishContext context)
        {
            ITile selectedTile = command.SelectedTile;
            if (selectedTile.CurrentState != ETileState.Movable) return;
          
            entity.ChangeState(entity.MoveState, () => new PlayerMoveStateData(){TargetTile = selectedTile});
        }

        public override void OnExit()
        {
            base.OnExit();
            foreach(var disposable in _disposables) disposable.Dispose();
            _disposables.Clear();
        }
    }
}