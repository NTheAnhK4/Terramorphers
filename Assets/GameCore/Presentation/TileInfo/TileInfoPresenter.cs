using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using WEngine.MVP;

namespace GameCore.Presentation.TileInfo
{
    public class TileInfoPresenter : ActivityPresenter<TileInfoActivity, TileInfoViewState>
    {
        private string _title, _description;
        private Vector3 _position;
        private bool _isDisplayRight;
        public TileInfoPresenter(TileInfoActivity activity, string title, string description, Vector3 position, bool isDisplayRight) : base(activity)
        {
            _title = title;
            _description = description;
            _position = position;
            _isDisplayRight = isDisplayRight;
        }

        protected override UniTask Initialize(Memory<object> args, TileInfoViewState state, TileInfoActivity view)
        {
            base.Initialize(args, state, view);
            state.IsDisplayRight = _isDisplayRight;
            state.Description.Value = _description;
            state.Title.Value = _title;
            state.WorldPos.Value = _position;
            return UniTask.CompletedTask;
        }
        
    }
}