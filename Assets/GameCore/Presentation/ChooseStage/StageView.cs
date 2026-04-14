

using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;
using R3;
using TMPro;

namespace GameCore.Presentation.ChooseStage
{
    public class StageView : AppView<StageViewState>
    {
        [SerializeField] private TextMeshProUGUI stageNameText;
        [SerializeField] private GameObject lockGO;
        [SerializeField] private GameObject unlockGO;
        [SerializeField] private Button enterStageBtn;
        protected override UniTask Initialize(StageViewState state)
        {
            state.IsUnlock.Subscribe(UnlockStage).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void UnlockStage(bool isUnlock)
        {
            lockGO.gameObject.SetActive(!isUnlock);
            unlockGO.gameObject.SetActive(isUnlock);
        }

        private void SetStageName(string stageName) => stageNameText.text = stageName;
    }

}
