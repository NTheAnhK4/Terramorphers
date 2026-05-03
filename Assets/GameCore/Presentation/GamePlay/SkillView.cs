using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Utility;
using GameCore.Utility.UI;
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
        [SerializeField] private HoldButton skillButton;
        [SerializeField] private TextMeshProUGUI skillCosts;
        [SerializeField] private Image coverImage;
        [SerializeField] private HoldButton endWaitingButton;
        [SerializeField] private TextMeshProUGUI coolDownText;
        [SerializeField] private HoldButton coverButton;
        private static readonly int Brightness = Shader.PropertyToID("_Brightness");

        private void Awake()
        {
            if (skillImage.material != null) skillImage.material = Instantiate(skillImage.material);
        }

        protected override UniTask Initialize(SkillViewState state)
        {
            skillImage.sprite = state.SkillMetadata.SkillSprite;
            skillButton.OnClick.SubscribeToCommand(state.UseSkillCommand).AddTo(this);
            skillButton.IsHolding.SubscribeToReactiveProperty(state.ShowSkillInfo).AddTo(this);
            coverButton.IsHolding.SubscribeToReactiveProperty(state.ShowSkillInfo).AddTo(this);
            skillCosts.text = $"<sprite=8>{state.SkillMetadata.SkillCosts}" ;
            state.SkillState.Subscribe(ChangeState).AddTo(this);
            endWaitingButton.OnClick.SubscribeToCommand(state.EndWaitingCommand).AddTo(this);
            endWaitingButton.IsHolding.SubscribeToReactiveProperty(state.ShowSkillInfo).AddTo(this);
           
            state.CoolDown.Subscribe(SetCoolDownText).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void ChangeState(SkillViewPresenter.SkillState state)
        {
            skillButton.interactable = state == SkillViewPresenter.SkillState.Enable;
            DOTween.Kill(coverImage.transform);
            switch (state)
            {
                case SkillViewPresenter.SkillState.Enable:
                    skillImage.material.SetFloat(Brightness,1);
                    coverImage.transform.DOScaleX(0, .2f);
                    endWaitingButton.gameObject.SetActive(false);
                    break;
                case SkillViewPresenter.SkillState.Disable:
                    skillImage.material.SetFloat(Brightness,1);
                    coverImage.transform.DOScaleX(1, .2f);
                    endWaitingButton.gameObject.SetActive(false);
                    break;
                case SkillViewPresenter.SkillState.Waiting:
                    skillImage.material.SetFloat(Brightness,7);
                    endWaitingButton.gameObject.SetActive(true);
                    break;
                case SkillViewPresenter.SkillState.CoolDown:
                    skillImage.material.SetFloat(Brightness,1);
                    coverImage.transform.localScale = Vector3.one;
                    endWaitingButton.gameObject.SetActive(false);
                    break;
            }
        }

        private void SetCoolDownText(int value)
        {
            if(value == 0) coolDownText.gameObject.SetActive(false);
            else
            {
                coolDownText.text = value.ToString();
                coolDownText.gameObject.SetActive(true);
            }
        }

        
    }
}

