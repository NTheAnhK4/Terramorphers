using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WEngine.MVP;

namespace GameCore.Presentation.EntityInfo
{
    public class EntityInfoModal : Modal<EntityInfoViewState>, IPointerDownHandler
    {
        [SerializeField] private Image entityImage;

        [SerializeField] private TextMeshProUGUI tileTitleText;
        [SerializeField] private TextMeshProUGUI tileDescriptionText;
        
        [SerializeField] private List<StatView> _statViews = new();
        private EntityInfoViewState _state;

        public List<StatView> StatViews => _statViews;
        public override UniTask InitializeState(EntityInfoViewState state, Memory<object> args)
        {
            _state = state;
            SetSprite(state.EntitySprite);
            tileTitleText.text = state.TileTitle;
            tileDescriptionText.text = state.TileDescription;
            return UniTask.CompletedTask;
        }

        private void SetSprite(Sprite entitySprite)
        {
            entityImage.sprite = entitySprite;
            entityImage.SetNativeSize();
            RectTransform rt = entityImage.rectTransform;
            float height = entitySprite.rect.height;
            float scaledHeight = height / entityImage.pixelsPerUnit;
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, scaledHeight / 2f);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            if(results.Count() <= 2) _state.OnClose.Execute(default);
        }
    }
}