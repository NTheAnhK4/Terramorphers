using System.Collections.Generic;
using GameCore.Domain.Skill;
using R3;
using WEngine.MVP;

namespace GameCore.Presentation.HeroInfo.CurrentSkill
{
    public class CurrentSkillViewState : ViewState
    {
        public List<ReactiveProperty<SkillMetadata>> SkillMetadatas = new();
      
        public ReactiveCommand<int> SelectSkill { get; } = new();
    }
}