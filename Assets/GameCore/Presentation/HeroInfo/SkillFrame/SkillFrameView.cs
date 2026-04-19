using Cysharp.Threading.Tasks;

using GameCore.Domain.Skill;
using GameCore.Utility;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.HeroInfo.SkillFrame
{
    public class SkillFrameView : AppView<SkillFrameViewState>
    {
        [SerializeField] private Image skillImage;
        [SerializeField] private Image skillFrame;
        [SerializeField] private Image unselectImage;
        [SerializeField] private Button selectBtn;

        public Image UnselectImage => unselectImage;
        protected override UniTask Initialize(SkillFrameViewState state)
        {
            selectBtn.SubscribeToCommand(state.SelectCommand).AddTo(this);
            Setup(state.SkillMetadata);
            
            return UniTask.CompletedTask;
        }

        private void Setup(SkillMetadata skillMetadata)
        {
            skillImage.sprite = skillMetadata.SkillSprite;
        }

       
    }
}