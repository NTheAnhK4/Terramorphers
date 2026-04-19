using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using GameCore.Presentation.Skill;
using GameCore.Utility;
using R3;
using UnityEngine;
using WEngine.MVP;

namespace GameCore.Presentation.HeroInfo.SkillFrame
{
    public class SkillFramePresenter : AppViewPresenter<SkillFrameView,SkillFrameViewState>
    {
        private SkillMetadata _skillMetadata;
        private SkillInfoPresenter _skillInfoPresenter;
        private ReactiveCommand<int> _selectCommand;
        private int _id;
        private SkillFrameViewState _state;
       
        public SkillFramePresenter(SkillFrameView view, SkillMetadata skillMetadata, 
            ReactiveCommand<int> selectCommand, int id, SkillInfoPresenter skillInfoPresenter) : base(view)
        {
            _skillMetadata = skillMetadata;
            _skillInfoPresenter = skillInfoPresenter;
            _selectCommand = selectCommand;
            _id = id;
        }

        protected override UniTask Initialize(SkillFrameViewState state, SkillFrameView view)
        {
            _state = state;
            state.SkillMetadata = _skillMetadata;
            _selectCommand.Subscribe(SelectSkill).AddTo(view);
            state.SelectCommand.Subscribe(_ => _selectCommand.Execute(_id)).AddTo(view);
           
            return UniTask.CompletedTask;
        }

        private void SelectSkill(int frameID)
        {
            if (frameID == _id)
            {
                _skillInfoPresenter.SkillMetadata.Value = _skillMetadata;
                _skillInfoPresenter.ShowSkillInfo.Value = true;
                View.UnselectImage.gameObject.SetActive(false);
            }
            else
            {
                View.UnselectImage.gameObject.SetActive(true);
            }
           
        }
    }
}