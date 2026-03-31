using System;
using System.Collections.Generic;
using CoreGame;
using GameCore.Commands;
using Terramorphers.Command;
using UnityEngine;
using VitalRouter;

namespace Terramorphers.States.PlayerState
{
    public class SelectMoveTileState : State<Player>
    {

        private List<IDisposable> _disposables = new();
        public SelectMoveTileState(Player entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public SelectMoveTileState(Player entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.Publisher.PublishAsync(new EnableEndTurnCommand() { IsEnable = true });
            entity.Publisher.PublishAsync(new EnableSkillCommand() { IsEnable = true });
            entity.Publisher.PublishAsync(new SetMovableTilesCommand() { CenterTile = entity.CurrentTile, Distance = entity.RemainStamina });
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
            SelectSkillTileData selectSkillTileData = new SelectSkillTileData() { SkillID = command.SkillID };
            entity.ChangeState(entity.SelectSkillTileState, () => selectSkillTileData);
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
            foreach(var disposable in _disposables) disposable.Dispose();
            _disposables.Clear();
        }
    }
}