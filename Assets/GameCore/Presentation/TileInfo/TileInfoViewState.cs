using R3;
using WEngine.MVP;
using UnityEngine;
namespace GameCore.Presentation.TileInfo
{
    public class TileInfoViewState : ViewState
    {
        public ReactiveProperty<string> Title { get; } = new();
        public ReactiveProperty<string> Description { get; } = new();
        public ReactiveProperty<Vector3> WorldPos { get; } = new();
        public bool IsDisplayRight;
    }
}