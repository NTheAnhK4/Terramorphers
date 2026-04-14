using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WEngine.MVP
{
    public abstract class AppView<TState> : MonoBehaviour
        where TState : ViewState
    {
        private bool _isInitialized = false;

      
        public async UniTask InitializeAsync(TState state)
        {
            
            if (_isInitialized)
                return;

            _isInitialized = true;
            await Initialize(state);
        }

        protected abstract UniTask Initialize(TState state);
    }
}