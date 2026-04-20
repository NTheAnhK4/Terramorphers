using Cysharp.Threading.Tasks;

using GameCore.Domain.Skill;
using GameCore.Utility;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;
using R3;
using UnityEngine.Serialization;

namespace GameCore.Presentation.HeroInfo.SkillFrame
{
    public class SkillFrameView : AppView<SkillFrameViewState>
    {
        [SerializeField] private Image skillImage;
        [SerializeField] private Image skillFrame;
        [FormerlySerializedAs("unselectImage")] [SerializeField] private Image unpreviewImage;
        [SerializeField] private Button selectBtn;
        [SerializeField] private Image selectImage;

        public Image SelectImage => selectImage;


        public Image UnpreviewImage => unpreviewImage;
        protected override UniTask Initialize(SkillFrameViewState state)
        {
            selectBtn.SubscribeToCommand(state.PreviewSkillCommand).AddTo(this);
            Setup(state.SkillMetadata);
            
            return UniTask.CompletedTask;
        }

        private void Setup(SkillMetadata skillMetadata)
        {
            skillImage.sprite = skillMetadata.SkillSprite;
        }

       
    }
}