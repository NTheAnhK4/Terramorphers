
using R3;
using WEngine.MVP;

namespace GameCore.Presentation.HeroInfo
{
    public class HeroInfoViewState : ViewState
    {
        public ReactiveCommand ExitCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ChooseSkillCommand { get; } = new ReactiveCommand();
    }

}
