using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Domain.Shared;
using GameCore.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.GamePlay
{
    public class SkillView : AppView<SkillViewState>
    {
        [SerializeField] private Image skillImage;
        [SerializeField] private Button skillButton;
        [SerializeField] private TextMeshProUGUI skillCosts;
        [SerializeField] private Image coverImage;
        protected override UniTask Initialize(SkillViewState state)
        {
            skillImage.sprite = state.SkillMetadata.SkillSprite;
            skillButton.SubscribeToCommand(state.UseSkillCommand).AddTo(this);
            skillCosts.text = state.SkillMetadata.SkillCosts.ToString();
            state.EnableUseSkill.Subscribe(EnableUseSkill).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void EnableUseSkill(bool isOn)
        {
            skillButton.interactable = isOn;
            DOTween.Kill(coverImage.transform);
            coverImage.transform.DOScaleX(isOn ? 0 : 1, .2f);
        }
    }
}

