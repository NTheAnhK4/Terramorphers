using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using GameCore.Utility;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using WEngine.MVP;
using R3;
namespace GameCore.Presentation.Lobby{
    public class LobbyScreen : Screen<LobbyViewState>
    {
        [SerializeField, TabGroup("Data")] private WorldCellView worldCellPrefab;
        [SerializeField, TabGroup("Components")]
        private Transform worldHolder;

        [SerializeField, TabGroup("Components")]
        private ScrollRect scrollRect;

        [SerializeField, TabGroup("Components")]
        private Button heroInfoBtn;

        private List<RectTransform> worldCellRects = new();
        public override UniTask InitializeState(LobbyViewState state, Memory<object> args)
        {
            state.CurrentIndex.Subscribe(ScrollTo).AddTo(this);
            heroInfoBtn.SubscribeToCommand(state.ShowHeroInfo).AddTo(this);
            return UniTask.CompletedTask;
        }

        public WorldCellPresenter CreateWorldCell(int levelID,LevelMetadata levelMetadata)
        {
            var worldCell = Instantiate(worldCellPrefab, worldHolder);
            worldCell.transform.name = levelMetadata.LevelName;
            WorldCellPresenter worldCellPresenter = new WorldCellPresenter(worldCell,levelID, levelMetadata);
            worldCellRects.Add(worldCell.RectTransform);
            return worldCellPresenter;
        }

        private void ScrollTo(int index)
        {
            Canvas.ForceUpdateCanvases();

            var content = scrollRect.content;
            var viewport = scrollRect.viewport;
            var target = worldCellRects[index];

          
            Vector2 viewportLocalPos = (Vector2)viewport.InverseTransformPoint(viewport.position);
            Vector2 targetLocalPos = (Vector2)viewport.InverseTransformPoint(target.position);

            float offset = viewportLocalPos.y - targetLocalPos.y;

            Vector2 newPos = content.anchoredPosition;
            newPos.y += offset;

            content.anchoredPosition = newPos;
        }
    }

}
