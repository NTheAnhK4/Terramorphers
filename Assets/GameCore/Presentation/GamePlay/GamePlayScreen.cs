
using System;
using System.Collections.Generic;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using GameCore.Presentation.Skill;
using GameCore.Utility;
using R3;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using WEngine.MVP;
using UnityEngine.UI;
using VContainer;

namespace GameCore.Presentation.GamePlay
{
    public class GamePlayScreen : Screen<GamePlayViewState>
    {
        [SerializeField, TabGroup("Skills")]
        private List<SkillView> _skillViews = new();

        [SerializeField, TabGroup("Resources")]
        private TextMeshProUGUI manaText;

        [SerializeField, TabGroup("Resources")]
        private TextMeshProUGUI staminaText;

        [SerializeField, TabGroup("Turn")]
        private TextMeshProUGUI turnText;

        [SerializeField, TabGroup("Turn")]
        private TextMeshProUGUI roundAmountText;

        [SerializeField, TabGroup("Actions")]
        private Button continueBtn;

        [SerializeField, TabGroup("Actions")]
        private Button objectiveBtn;

        [SerializeField, TabGroup("Actions")]
        private Button exitBtn;

        [SerializeField, TabGroup("Actions")]
        private Button endTurnBtn;

        [SerializeField, TabGroup("Actions")]
        private Image coverEndTurnBtn;

        [SerializeField, TabGroup("Skills")]
        private SkillInfoView skillInfoView;

        [SerializeField, TabGroup("Resources")]
        private Image staminaFill;

        [SerializeField, TabGroup("Resources")]
        private Image manaFill;

        [SerializeField, TabGroup("Turn")]
        private TurnNotificationAnimation _turnNotificationAnimation;

        private static readonly int Fill = Shader.PropertyToID("_Fill");

        public List<SkillView> SkillViews => _skillViews;


        public override UniTask InitializeState(GamePlayViewState state, Memory<object> args)
        {
            endTurnBtn.SubscribeToCommand(state.EndTurnCommand);
            state.IsActiveEndTurnCommand.Subscribe(ToggleEndTurnButton).AddTo(this);
            state.CurrentRound.Subscribe(SetRound).AddTo(this);
            state.Stamina.Subscribe(OnStaminaChange).AddTo(this);
            state.Mana.Subscribe(OnManaChange).AddTo(this);
            
           
            exitBtn.SubscribeToCommand(state.ExitCommand).AddTo(this);
            objectiveBtn.SubscribeToCommand(state.ObjectiveCommand).AddTo(this);

           
            return UniTask.CompletedTask;
        }

        public SkillInfoPresenter InitSkillInfoPresenter()
        {
            skillInfoView.gameObject.SetActive(false);
            SkillInfoPresenter skillInfoPresenter = new SkillInfoPresenter(skillInfoView);
            return skillInfoPresenter;
        }

       
        private void ToggleEndTurnButton(bool isOn)
        {
            coverEndTurnBtn.gameObject.SetActive(!isOn);
        }

        private void SetRound(int round)
        {
            _turnNotificationAnimation.Show();
            roundAmountText.text = $"Round : {round}";
        }

        private void OnStaminaChange((int stamina, int maxStamina) value)
        {
            staminaText.text = value.stamina.ToString();
            float fillTarget;
            if (value.maxStamina == 0) fillTarget = 1;
            else fillTarget = 1.0f * value.stamina / value.maxStamina;
            DOTween.Kill(staminaFill.transform);
            staminaFill.DOFillAmount(fillTarget, .1f).SetTarget(staminaFill.transform);
        }

        private void OnManaChange((int mana, int maxMana) value)
        {
            manaText.text = value.mana.ToString();
            float fillTarget;
            if (value.maxMana == 0) fillTarget = 1;
            else fillTarget = 1.0f * value.mana / value.maxMana;
            
            DOTween.Kill(manaFill.material);

            DOVirtual.Float(
                manaFill.material.GetFloat(Fill),
                fillTarget,
                0.15f,
                v =>
                {
                    manaFill.material.SetFloat(Fill, v);
                }
            ).SetTarget(manaFill.material);
        }

       
    }

}
