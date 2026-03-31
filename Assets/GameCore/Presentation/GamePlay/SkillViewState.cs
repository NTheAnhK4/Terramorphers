using GameCore.Domain.Skill;
using R3;
using WEngine.MVP;

namespace GameCore.Presentation.GamePlay
{
    public class SkillViewState : ViewState
    {
        public SkillMetadata SkillMetadata;
        public ReactiveCommand UseSkillCommand { get; } = new ReactiveCommand();
        public ReactiveProperty<bool> EnableUseSkill { get; } = new ReactiveProperty<bool>();
    }
}