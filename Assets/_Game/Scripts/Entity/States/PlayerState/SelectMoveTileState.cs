using System;
using CoreGame;

using Terramorphers.Command;
using UnityEngine;
using VitalRouter;

namespace Terramorphers.States.PlayerState
{
    public class SelectMoveTileState : State<Player>
    {

        private IDisposable _disposable;
        public SelectMoveTileState(Player entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public SelectMoveTileState(Player entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
          
            entity.Publisher.PublishAsync(new SetMovableTilesCommand() { CenterTile = entity.CurrentTile, Distance = entity.RemainMoveDistance });
            _disposable = entity.Subscribable.Subscribe<SelectTileCommand>(OnSelectTile);
        }

        public override void Update()
        {
            base.Update();
            entity.InputManager.OnUpdate();
        }

        private void OnSelectTile(SelectTileCommand command, PublishContext context)
        {
            ITile selectedTile = command.SelectedTile;
            if (selectedTile.CurrentState != ETileState.Movable) return;
            entity.SelectedTile = selectedTile;
            entity.ChangeState(entity.MoveState);
        }

        public override void OnExit()
        {
            base.OnExit();
            _disposable?.Dispose();
        }
    }
}