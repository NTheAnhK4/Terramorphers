using System;
using System.Collections.Generic;
using CoreGame;
using GameCore.Commands;
using GameCore.Domain.Skill;
using Terramorphers.Command;
using UnityEngine;
using VitalRouter;

namespace Terramorphers.States.PlayerState
{
    public class SelectSkillTileData : StateData
    {
        public int SkillID { get; set; }
    }
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
            if (stateData == null || stateData is not SelectSkillTileData selectSkillTileData)
            {
                Debug.Log($"[Test] type of data for select skill Applicable is not correct");
                return;
            }

            var skillMetadata = entity.SkillManager.GetSkillMetadata(selectSkillTileData.SkillID);
            IReadOnlyList<ESkillTargetType> skillTargetTypes = skillMetadata.SkillTargetTypes;

            entity.Publisher.PublishAsync(new EnableEndTurnCommand() { IsEnable = true });
            int newRange = skillMetadata.Range == 0 ? 1 : skillMetadata.Range + entity.StatsSystem.Stats.Range;
            
            entity.Publisher.PublishAsync(new SetSkillApplicableTilesCommand()
            {
                Entity = entity,
                Distance = newRange,
                SkillTargetTypes = skillTargetTypes,
            });
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