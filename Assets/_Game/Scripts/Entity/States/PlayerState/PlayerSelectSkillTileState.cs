using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame;
using GameCore.Commands;
using GameCore.Domain.Skill;
using Terramorphers.Command;
using UnityEngine;
using UnityEngine.Assertions.Must;
using VitalRouter;

namespace Terramorphers.States.PlayerState
{
    public class SelectSkillTileData : StateData
    {
        public int SkillID { get; set; }
    }
    public class PlayerSelectSkillTileState : State<Player>
    {
        private List<IDisposable> _disposables = new();
        private List<ETileState> _tileStates = new();
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

           
            _disposables.Add(entity.Subscribable.Subscribe<SelectTileCommand>(OnSelectTile));
            _disposables.Add(entity.Subscribable.Subscribe<UseSkillCommand>(OnUseSkill));
           
            HandleSkillSelected(selectSkillTileData.SkillID);
        }

        private void OnUseSkill(UseSkillCommand command, PublishContext context)
        {
            HandleSkillSelected(command.SkillID);
        }

        private void HandleSkillSelected(int skillID)
        {
            if (skillID < 0)
            {
                entity.ChangeState(entity.SelectMoveTileState);
                return;
            }
            var skillMetadata = entity.SkillManager.GetSkillMetadata(skillID);
            IReadOnlyList<ESkillTargetType> skillTargetTypes = skillMetadata.SkillTargetTypes;
            
            FromSkillTargetToTileState(skillTargetTypes);
            
            entity.Publisher.PublishAsync(new EnableEndTurnCommand() { IsEnable = true });
            int newRange = skillMetadata.Range == 0 ? 1 : skillMetadata.Range + entity.StatsSystem.Stats.Range;
            
            entity.Publisher.PublishAsync(new SetSkillApplicableTilesCommand()
            {
                Entity = entity,
                Distance = newRange,
                SkillTargetTypes = skillTargetTypes,
            });
        }

        void FromSkillTargetToTileState(IReadOnlyList<ESkillTargetType> skillTargetTypes)
        {
            _tileStates.Clear();
            foreach (var skillTarget in skillTargetTypes)
            {
                switch (skillTarget)
                {
                    case ESkillTargetType.Self:
                        _tileStates.Add(ETileState.SelfTargetSkill);
                        break;
                    case ESkillTargetType.Enemy:
                        _tileStates.Add(ETileState.EnemyTargetSkill);
                        break;
                    case ESkillTargetType.Ally:
                        _tileStates.Add(ETileState.AllyTargetSkill);
                        break;
                    case ESkillTargetType.Tile:
                        _tileStates.Add(ETileState.TileTargetSkill);
                        break;
                }
            }

        }
        public override void Update()
        {
            base.Update();
           
            entity.InputManager.OnUpdate();
        }
        private void OnSelectTile(SelectTileCommand command, PublishContext context)
        {
            ITile selectedTile = command.SelectedTile;
            if (!_tileStates.Contains(selectedTile.CurrentState)) return;
         
            entity.SelectedTile = selectedTile;
            
        }
        public override void OnExit()
        {
            base.OnExit();
            foreach(var disposable in _disposables) disposable?.Dispose();
        }
    }
}