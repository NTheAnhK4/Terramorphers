using System;
using CoreGame;
using GameCore.Commands;
using Terramorphers.Command;
using VitalRouter;

namespace Terramorphers.States.PlayerState
{
    public class PlayerSelectSkillTileState : State<Player>
    {
        private IDisposable _disposable;
        public PlayerSelectSkillTileState(Player entity, string animBoolName) : base(entity, animBoolName)
        {
        }

        public PlayerSelectSkillTileState(Player entity, Func<string> animNameFunc) : base(entity, animNameFunc)
        {
        }

        public override void OnEnter(StateData stateData = null)
        {
            base.OnEnter(stateData);
            entity.Publisher.PublishAsync(new ToggleEndTurnCommand() { IsOn = true });
            entity.Publisher.PublishAsync(new SetSkillApplicableTilesCommand() { CenterTile = entity.CurrentTile, Distance = 4});
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
            //check target skill
            if (selectedTile.CurrentState != ETileState.SkillApplicable) return;
            entity.SelectedTile = selectedTile;
            //entity.ChangeState(entity.MoveState);
        }
        public override void OnExit()
        {
            base.OnExit();
            _disposable?.Dispose();
        }
    }
}