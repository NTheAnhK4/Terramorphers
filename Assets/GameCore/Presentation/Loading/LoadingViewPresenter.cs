using System;
using Cysharp.Threading.Tasks;
using GameCore.Presentation.Shared;
using R3;
using UnityEngine;
using VContainer;
using WEngine.MVP;

namespace GameCore.Presentation.Loading
{
    public class LoadingViewPresenter : ActivityPresenter<LoadingView, LoadingViewState>
    {
        [Inject] private TransitionService _transitionService;
        private ReactiveProperty<float> _progress;
        private Action _onFinishLoading;
        public LoadingViewPresenter(LoadingView view, ReactiveProperty<float> progress, Action onFinishLoading) : base(view)
        {
            _progress = progress;
            _onFinishLoading = onFinishLoading;
        }

        protected override UniTask Initialize(Memory<object> args, LoadingViewState state, LoadingView view)
        {
            base.Initialize(args, state, view);
            state.CloseActivity.Subscribe(_ => CloseActivity().Forget()).AddTo(view);
            _progress.Subscribe(value => state.Progress.Value = value).AddTo(view);
            return UniTask.CompletedTask;
        }

        private async UniTask CloseActivity()
        {
           
            await _transitionService.ShowLoadingView(false, null, null);
          
            _onFinishLoading?.Invoke();
        }

        public UniTask GetLoading() => View.Loading();
        public UniTask GetPreLoading() => View.PreLoading();
    }
}