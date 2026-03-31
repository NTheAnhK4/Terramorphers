
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
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
        [SerializeField, TabGroup("Components")]
        private List<SkillView> _skillViews = new();
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

        private void SetRound(int round) => roundAmountText.text = $"Round : {round}";
    }

}
