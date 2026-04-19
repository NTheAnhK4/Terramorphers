using GameCore.Domain.Skill;
using R3;
using WEngine.MVP;

namespace GameCore.Presentation.Skill
{
    public class SkillInfoViewState : ViewState
    {
        public ReactiveProperty<SkillMetadata> skillMetaData { get; } = new();
        public ReactiveProperty<bool> ShowSkillInfoCommand { get; } = new();
    }
}