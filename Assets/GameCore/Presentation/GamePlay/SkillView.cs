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
        [SerializeField] private Button endWaitingButton;
        protected override UniTask Initialize(SkillViewState state)
        {
            skillImage.sprite = state.SkillMetadata.SkillSprite;
            skillButton.SubscribeToCommand(state.UseSkillCommand).AddTo(this);
            skillCosts.text = state.SkillMetadata.SkillCosts.ToString();
            state.SkillState.Subscribe(ChangeState).AddTo(this);
            endWaitingButton.SubscribeToCommand(state.EndWaitingCommand).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void ChangeState(SkillViewPresenter.SkillState state)
        {
            skillButton.interactable = state == SkillViewPresenter.SkillState.Enable;
            DOTween.Kill(coverImage.transform);
            switch (state)
            {
                case SkillViewPresenter.SkillState.Enable:
                    coverImage.transform.DOScaleX(0, .2f);
                    endWaitingButton.gameObject.SetActive(false);
                    break;
                case SkillViewPresenter.SkillState.Disable:
                    coverImage.transform.DOScaleX(1, .2f);
                    endWaitingButton.gameObject.SetActive(false);
                    break;
                case SkillViewPresenter.SkillState.Waiting:
                    endWaitingButton.gameObject.SetActive(true);
                    break;
            }
        }

        
    }
}

