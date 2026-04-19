using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using GameCore.Utility;
using R3;
using WEngine.MVP;

namespace GameCore.Presentation.Skill
{
    public class SkillInfoPresenter : AppViewPresenter<SkillInfoView, SkillInfoViewState>
    {
        public ReactiveProperty<SkillMetadata> SkillMetadata { get; } = new();
        public ReactiveProperty<bool> ShowSkillInfoCommand { get; } = new();
        private SkillInfoViewState _state;
        public SkillInfoPresenter(SkillInfoView view) : base(view)
        {
        }

        protected override UniTask Initialize(SkillInfoViewState state, SkillInfoView view)
        {
            _state = state;
            SkillMetadata.SubscribeToReactiveProperty(state.skillMetaData).AddTo(view);
            ShowSkillInfoCommand.SubscribeToReactiveProperty(state.ShowSkillInfoCommand).AddTo(view);
            return UniTask.CompletedTask;
        }

       
    }
}