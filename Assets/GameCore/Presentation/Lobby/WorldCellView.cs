using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using GameCore.Utility;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.Lobby
{
    public class WorldCellView : AppView<WorldCellViewState>
    {
        [SerializeField] private Image lockImg;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Button enterWorldBtn;
        [SerializeField] private Image worldImage;

        public RectTransform RectTransform => _rectTransform;
        protected override UniTask Initialize(WorldCellViewState state)
        {
            state.LevelMetadata.Subscribe(SetupWorldCell).AddTo(this);
            state.IsLevelUnlock.Subscribe(SetUnlock).AddTo(this);
            enterWorldBtn.SubscribeToCommand(state.EnterWorldCommand).AddTo(this);
            return UniTask.CompletedTask;
        }

        private void SetupWorldCell(LevelMetadata levelMetadata)
        {
            if (levelMetadata == null) return;
            worldImage.sprite = levelMetadata.LevelSprite;
        }

        private void SetUnlock(bool isUnlock)
        {
            enterWorldBtn.interactable = isUnlock;
            lockImg.gameObject.SetActive(!isUnlock);
        }
    }
}


