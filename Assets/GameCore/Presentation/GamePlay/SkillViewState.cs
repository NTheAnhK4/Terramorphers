using GameCore.Domain.Skill;
using R3;
using WEngine.MVP;

namespace GameCore.Presentation.GamePlay
{
    public class SkillViewState : ViewState
    {
        public SkillMetadata SkillMetadata;
        public ReactiveCommand UseSkillCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<SkillViewPresenter.SkillState> SkillState { get; } = new ReactiveProperty<SkillViewPresenter.SkillState>();
        public ReactiveCommand EndWaitingCommand { get; } = new();
    }
}