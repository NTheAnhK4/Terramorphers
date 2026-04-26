using R3;
using UnityEngine;
using WEngine.MVP;

namespace GameCore.Presentation.EntityInfo
{
    public class EntityInfoViewState : ViewState
    {
        public ReactiveCommand OnClose { get; } = new();
        public string TileDescription;
        public string TileTitle;
        public Sprite EntitySprite;
    }
}