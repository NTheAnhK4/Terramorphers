

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Entity;
using GameCore.Presentation.Item;
using GameCore.Utility;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;
using R3;
using TMPro;

namespace GameCore.Presentation.ChooseStage
{
    public class StageView : AppView<StageViewState>
    {
        [SerializeField] private Sprite starOn, starOff;
        [SerializeField] private TextMeshProUGUI stageNameText;
        [SerializeField] private GameObject lockGO;
        [SerializeField] private GameObject unlockGO;
        [SerializeField] private Button enterStageBtn;
        [SerializeField] private List<ItemView> _enemyFrameViews = new();
        [SerializeField] private List<Image> stars = new();

    
        protected override UniTask Initialize(StageViewState state)
        {
           
            state.IsUnlock.Subscribe(UnlockStage).AddTo(this);
            state.StageName.Subscribe(SetStageName).AddTo(this);
            SetStar(state.TotalStars);
            enterStageBtn.SubscribeToCommand(state.EnterStageCommand).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void SetStar(int value)
        {
            for (int i = 0; i < stars.Count; ++i)
            {
                stars[i].sprite = i < value ? starOn :
                starOff;
                
            }
        }

       

        private void UnlockStage(bool isUnlock)
        {
            lockGO.gameObject.SetActive(!isUnlock);
            unlockGO.gameObject.SetActive(isUnlock);
        }

        private void SetStageName(string stageName) => stageNameText.text = stageName;

        public void ShowEnemies(List<BaseEntityMetadata> baseEntityMetadatas)
        {
            for (int i = 0; i < _enemyFrameViews.Count; ++i)
            {
                if(i < baseEntityMetadatas.Count) _enemyFrameViews[i].Init(baseEntityMetadatas[i].EntityIcon);
                _enemyFrameViews[i].gameObject.SetActive(i < baseEntityMetadatas.Count);
            }
        }
}

}
