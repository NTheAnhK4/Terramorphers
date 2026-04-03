using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Domain.Skill;
using GameCore.Utility;
using UnityEngine;
using WEngine.MVP;
using R3;
using VContainer;
using VitalRouter;

namespace GameCore.Presentation.GamePlay
{
    public class SkillViewPresenter : AppViewPresenter<SkillView, SkillViewState>
    {
        private SkillMetadata _skillMetadata;
        [Inject] private ICommandSubscribable _subscribable;
        [Inject] private ICommandPublisher _publisher;
        private SkillViewState _state;
        private int currentMana;

        public enum SkillState
        {
            Enable,
            Waiting,
            Disable
        }

        
       
        public SkillViewPresenter(SkillView view, SkillMetadata skillMetadata) : base(view)
        {
            _skillMetadata = skillMetadata;
        }

        protected override UniTask Initialize(SkillViewState state, SkillView view)
        {
            _state = state;
            _subscribable.Subscribe<EnableSkillCommand>(EnableUseSkill);
            _subscribable.Subscribe<UseSkillCommand>(OnUseSkill).AddTo(view);
            _subscribable.Subscribe<ChangePlayerManaCommand>(OnManaChange).AddTo(view);
            state.SkillMetadata = _skillMetadata;
            state.EndWaitingCommand.Subscribe(OnEndWaiting).AddTo(view);
            state.UseSkillCommand.Subscribe(_ => _publisher.PublishAsync(new UseSkillCommand()
                {
                    SkillID = _skillMetadata.SkillID
                })).AddTo(view);
            return UniTask.CompletedTask;
        }

        private void OnManaChange(ChangePlayerManaCommand command, PublishContext context)
        {
            currentMana = command.Mana;
        }

        private void OnEndWaiting(Unit _)
        {
            ChangeState(SkillState.Enable);
            _publisher.PublishAsync(new UseSkillCommand() { SkillID = -1 });
        }

        private void OnUseSkill(UseSkillCommand command, PublishContext context)
        {
            if(command.SkillID == _skillMetadata.SkillID) ChangeState(SkillState.Waiting);
            else ChangeState(SkillState.Enable);
        }

        private void EnableUseSkill(EnableSkillCommand command, PublishContext context)
        {
            if (command.IsEnable)
            {
                if(_skillMetadata.SkillCosts <= currentMana) ChangeState(SkillState.Enable);
                else ChangeState(SkillState.Disable);
            }
            else ChangeState(SkillState.Disable);
        } 

        public void ChangeState(SkillState newState)
        {
           
            if (_state.SkillState.Value == SkillState.Disable && newState == SkillState.Waiting) return;
            _state.SkillState.Value = newState;
        }
    }
}