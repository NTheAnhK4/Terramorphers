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
        [SerializeField] private Image unpreviewImage;
        [SerializeField] private Button selectBtn;
        [SerializeField] private Image selectImage;
        [SerializeField] private Image lockImage;
        public Image SelectImage => selectImage;


        public Image UnpreviewImage => unpreviewImage;
        protected override UniTask Initialize(SkillFrameViewState state)
        {
            selectBtn.SubscribeToCommand(state.PreviewSkillCommand).AddTo(this);
            Setup(state.SkillMetadata);
            state.IsLock.Subscribe(SetLock).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void Setup(SkillMetadata skillMetadata)
        {
            skillImage.sprite = skillMetadata.SkillSprite;
        }

        private void SetLock(bool isLock) => lockImage.gameObject.SetActive(isLock);


    }
}