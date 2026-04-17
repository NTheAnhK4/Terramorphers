
using System;
using System.Collections.Generic;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using GameCore.Utility;
using R3;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using WEngine.MVP;
using UnityEngine.UI;
using VContainer;

namespace GameCore.Presentation.GamePlay
{
    public class GamePlayScreen : Screen<GamePlayViewState>
    {
        [SerializeField, TabGroup("Components")]
        private List<SkillView> _skillViews = new();
        [SerializeField, TabGroup("Components")] private TextMeshProUGUI manaText;
        [SerializeField, TabGroup("Components")] private TextMeshProUGUI staminaText;
        [SerializeField, TabGroup("Components")] private TextMeshProUGUI turnText;
        [SerializeField, TabGroup("Components")] private TextMeshProUGUI roundAmountText;
        [SerializeField, TabGroup("Components")] private Button continueBtn, listBtn,speedBtn, objectiveBtn,exitBtn, endTurnBtn;
        [SerializeField, TabGroup("Components")] private Image coverEndTurnBtn;

        [SerializeField, TabGroup("Components)")]
        private List<CanvasGroup> dropDownBtnLists = new();

        [SerializeField, TabGroup("Components")]
        private Image pannelDropDownBtn;

        [SerializeField, TabGroup("Components")]
        private Image staminaFill, manaFill;

        [SerializeField, TabGroup("Components")]
        private TurnNotificationAnimation _turnNotificationAnimation;
        [SerializeField, TabGroup("Components")] private List<Image> skillImages = new();
       
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

        public void InitSkillView(IObjectResolver resolver, List<SkillMetadata> skillMetadatas)
        {
            for (int i = 0; i < _skillViews.Count; ++i)
            {
                if(i >= skillMetadatas.Count) _skillViews[i].gameObject.SetActive(false);
                else
                {
                    _skillViews[i].gameObject.SetActive(true);
                    SkillViewPresenter skillViewPresenter = new SkillViewPresenter(_skillViews[i], skillMetadatas[i]);
                    resolver.Inject(skillViewPresenter);
                    skillViewPresenter.Initialize();
                }
            }
           
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
            DOTween.Kill(manaFill.transform);
            manaFill.DOFillAmount(fillTarget, .1f).SetTarget(manaFill.transform);
        }

       
    }

}
