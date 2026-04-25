using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore.Utility.UI;
using R3;
using TMPro;
using UnityEngine;
using WEngine.MVP;

namespace GameCore.Presentation.TileInfo
{
    public class TileInfoActivity : Activity<TileInfoViewState>
    {
        [SerializeField] private RectTransform holder;
        [SerializeField] private TextMeshProUGUI titleText, descriptionText;
        private TileInfoViewState _state;
        private void SetTitle(string value) => titleText.text = value;
        private void SetDescription(string value) => descriptionText.text = value;

        private void SetAnchoredPosition(Vector3 worldPos)
        {
           
            Vector2 anchoredPosition = UIUtility.WorldPosToAnchoredPosition((RectTransform)(transform),worldPos);
           
            float spacing = 25;
            if (_state.IsDisplayRight) anchoredPosition.x -= holder.sizeDelta.x / 2 + spacing;
            else  anchoredPosition.x += holder.sizeDelta.x / 2 + spacing;
            anchoredPosition.y += holder.sizeDelta.y / 2;
            holder.anchoredPosition = anchoredPosition;
            
        }

     
        public override  UniTask InitializeState(TileInfoViewState state, Memory<object> args)
        {
            _state = state;
            state.Description.Subscribe(SetDescription).AddTo(this);
            state.Title.Subscribe(SetTitle).AddTo(this);
            state.WorldPos.Subscribe(SetAnchoredPosition).AddTo(this);
            SetDescription(state.Description.Value);
            SetTitle(state.Title.Value);
            SetAnchoredPosition(state.WorldPos.Value);
           
            return UniTask.CompletedTask;
        }

       

       
        
    }

}
