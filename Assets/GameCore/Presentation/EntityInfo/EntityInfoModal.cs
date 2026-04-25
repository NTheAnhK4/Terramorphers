using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using WEngine.MVP;

namespace GameCore.Presentation.EntityInfo
{
    public class EntityInfoModal : Modal<EntityInfoViewState>, IPointerDownHandler
    {
        [SerializeField] private List<StatView> _statViews = new();
        private EntityInfoViewState _state;

        public List<StatView> StatViews => _statViews;
        public override UniTask InitializeState(EntityInfoViewState state, Memory<object> args)
        {
            _state = state;
            return UniTask.CompletedTask;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _state.OnClose.Execute(default);
        }
    }
}