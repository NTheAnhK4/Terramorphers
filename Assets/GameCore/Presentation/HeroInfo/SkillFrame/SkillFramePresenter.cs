using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using GameCore.Presentation.Skill;
using R3;
using WEngine.MVP;

namespace GameCore.Presentation.HeroInfo.SkillFrame
{
    public class SkillFramePresenter : AppViewPresenter<SkillFrameView,SkillFrameViewState>
    {
        private SkillMetadata _skillMetadata;
        private SkillInfoPresenter _skillInfoPresenter;
        private ReactiveCommand<int> _previewSkillCommand;
        private int _id;
        private SkillFrameViewState _state;
        private ReactiveCommand<int> _selectSkillCommand;
        private ReactiveCommand<int> _unselectSkillCommand;
       
        public SkillFramePresenter(SkillFrameView view, SkillMetadata skillMetadata, 
            ReactiveCommand<int> previewSkillCommand, int id, SkillInfoPresenter skillInfoPresenter,
            ReactiveCommand<int> selectSkillCommand, ReactiveCommand<int> unselectSkillCommand) : base(view)
        {
            _skillMetadata = skillMetadata;
            _skillInfoPresenter = skillInfoPresenter;
            _previewSkillCommand = previewSkillCommand;
            _selectSkillCommand = selectSkillCommand;
            _unselectSkillCommand = unselectSkillCommand;
            _id = id;
        }

        protected override UniTask Initialize(SkillFrameViewState state, SkillFrameView view)
        {
            _state = state;
            state.SkillMetadata = _skillMetadata;
            _previewSkillCommand.Subscribe(PreviewSkill).AddTo(view);
            state.PreviewSkillCommand.Subscribe(_ => _previewSkillCommand.Execute(_id)).AddTo(view);
            _selectSkillCommand.Subscribe(SelectSkill).AddTo(view);
            _unselectSkillCommand.Subscribe(UnselectSkill).AddTo(view);
            return UniTask.CompletedTask;
        }

        private void PreviewSkill(int frameID)
        {
            if (frameID == _id)
            {
                _skillInfoPresenter.SkillMetadata.Value = _skillMetadata;
                _skillInfoPresenter.ShowSkillInfo.Value = true;
                View.UnpreviewImage.gameObject.SetActive(false);
            }
            else
            {
                View.UnpreviewImage.gameObject.SetActive(true);
            }
           
        }

        private void SelectSkill(int skillID)
        {
            if (skillID != _id) return;
            View.SelectImage.gameObject.SetActive(true);
        }

        private void UnselectSkill(int skillID)
        {
            if (skillID != _id) return;
            View.SelectImage.gameObject.SetActive(false);
        }
    }
}