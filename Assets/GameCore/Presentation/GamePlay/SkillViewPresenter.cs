using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Domain.Skill;
using GameCore.Presentation.Skill;
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
        private SkillInfoPresenter _skillInfoPresenter;
       

        public enum SkillState
        {
            Enable,
            Waiting,
            Disable,
            CoolDown
        }

        
       
        public SkillViewPresenter(SkillView view, SkillMetadata skillMetadata, SkillInfoPresenter skillInfoPresenter) : base(view)
        {
            _skillMetadata = skillMetadata;
            _skillInfoPresenter = skillInfoPresenter;
        }

        protected override UniTask Initialize(SkillViewState state, SkillView view)
        {
            _state = state;
            _subscribable.Subscribe<EnableSkillCommand>(EnableUseSkill);
            _subscribable.Subscribe<UseSkillCommand>(OnUseSkill).AddTo(view);
            _subscribable.Subscribe<ChangePlayerManaCommand>(OnManaChange).AddTo(view);
            _subscribable.Subscribe<SkillCoolDownCommand>(OnSkillCoolDown).AddTo(view);
            state.SkillMetadata = _skillMetadata;
            state.EndWaitingCommand.Subscribe(OnEndWaiting).AddTo(view);
            state.UseSkillCommand.Subscribe(_ => _publisher.PublishAsync(new UseSkillCommand()
                {
                    SkillID = _skillMetadata.SkillID
                })).AddTo(view);
            state.ShowSkillInfo.Subscribe(ShowSkillInfo).AddTo(view);
            return UniTask.CompletedTask;
        }

        private void ShowSkillInfo(bool isShow)
        {
            if (isShow)  _skillInfoPresenter.SkillMetadata.Value = _skillMetadata;
            _skillInfoPresenter.ShowSkillInfo.Value = isShow;

        }

        private void OnSkillCoolDown(SkillCoolDownCommand command, PublishContext context)
        {
            if (command.SkillID != _skillMetadata.SkillID) return;
            _state.CoolDown.Value = command.CoolDown;
            
            if(command.CoolDown == 0) ChangeState(SkillState.Enable);
            else ChangeState(SkillState.CoolDown);
            
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
            if(command.IsEnable) ChangeState(_skillMetadata.SkillCosts <= currentMana ? SkillState.Enable : SkillState.Disable);
            else ChangeState(SkillState.Disable);
        } 

        public void ChangeState(SkillState newState)
        {
            
            if (_state.CoolDown.Value > 0)
            {
                if (_state.SkillState.Value != SkillState.CoolDown) _state.SkillState.Value = SkillState.CoolDown;
                return;
            }
            if (_state.SkillState.Value == SkillState.Disable && newState == SkillState.Waiting) return;
            if (newState == SkillState.Enable)
            {
                if (currentMana < _skillMetadata.SkillCosts) _state.SkillState.Value = SkillState.Disable;
                else _state.SkillState.Value = SkillState.Enable;
            }
            else _state.SkillState.Value = newState;
        }
    }
}