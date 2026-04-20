using GameCore.Domain.Skill;
using R3;
using WEngine.MVP;

namespace GameCore.Presentation.HeroInfo.SkillFrame
{
    public class SkillFrameViewState : ViewState
    {
        public ReactiveCommand PreviewSkillCommand { get; } = new ReactiveCommand();
       
        public SkillMetadata SkillMetadata;
       
    }
}