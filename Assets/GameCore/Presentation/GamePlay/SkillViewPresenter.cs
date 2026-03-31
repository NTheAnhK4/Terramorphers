using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Domain.Skill;
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
       
        public SkillViewPresenter(SkillView view, SkillMetadata skillMetadata) : base(view)
        {
            _skillMetadata = skillMetadata;
        }

        protected override UniTask Initialize(SkillViewState state, SkillView view)
        {
            _state = state;
            _subscribable.Subscribe<EnableSkillCommand>(EnableUseSkill);
            state.SkillMetadata = _skillMetadata;
            state.UseSkillCommand.Subscribe(OnUseSkill).AddTo(view);
            return UniTask.CompletedTask;
        }

        private void OnUseSkill(Unit _)
        {
            _publisher.PublishAsync(new UseSkillCommand() { SkillID = _skillMetadata.SkillID });
        }

        private void EnableUseSkill(EnableSkillCommand command, PublishContext context) => _state.EnableUseSkill.Value = command.IsEnable;
    }
}