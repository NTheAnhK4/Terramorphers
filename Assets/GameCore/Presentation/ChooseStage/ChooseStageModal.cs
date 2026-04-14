using System;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using TMPro;
using UnityEngine;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.ChooseStage
{
    public class ChooseStageModal : Modal<ChooseStageViewState>
    {
        [SerializeField] private Transform holder;
        [SerializeField] private StageView stagePrefab;
        [SerializeField] private TextMeshProUGUI levelText;
        public override UniTask InitializeState(ChooseStageViewState state, Memory<object> args)
        {
            state.LevelName.Subscribe(SetLevelName).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void SetLevelName(string value) => levelText.text = value;

        public StagePresenter CreateStage(int levelID,int stageID,LevelStageData levelStageData)
        {
            var stageView = Instantiate(stagePrefab, holder);
            Debug.Log($"[Test] {stageView.transform.position}");
            StagePresenter stagePresenter = new StagePresenter(stageView,levelID,stageID, levelStageData);
            return stagePresenter;
        }
    }

}
