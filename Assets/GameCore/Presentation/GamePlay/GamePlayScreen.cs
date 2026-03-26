
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Utility;
using R3;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using WEngine.MVP;
using UnityEngine.UI;

namespace GameCore.Presentation.GamePlay
{
    public class GamePlayScreen : Screen<GamePlayViewState>
    {
        [SerializeField, TabGroup("Text")] private TextMeshProUGUI manaText;
        [SerializeField, TabGroup("Text")] private TextMeshProUGUI staminaText;
        [SerializeField, TabGroup("Text")] private TextMeshProUGUI turnText;
        [SerializeField, TabGroup("Text")] private TextMeshProUGUI roundAmountText;
        [SerializeField, TabGroup("Button")] private Button continueBtn, settingBtn, endTurnBtn;
        [SerializeField, TabGroup("Image")] private Image coverEndTurnBtn;
        [SerializeField, TabGroup("Image")] private List<Image> skillImages = new();
       
        public override UniTask InitializeState(GamePlayViewState state, Memory<object> args)
        {
            endTurnBtn.SubscribeToCommand(state.EndTurnCommand);
            state.IsActiveEndTurnCommand.Subscribe(ToggleEndTurnButton).AddTo(this);
            state.CurrentRound.Subscribe(SetRound).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void ToggleEndTurnButton(bool isOn)
        {
            coverEndTurnBtn.gameObject.SetActive(!isOn);
        }

        private void SetRound(int round) => roundAmountText.text = $"Round : {round}";
    }

}
