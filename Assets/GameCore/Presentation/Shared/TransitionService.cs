using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Presentation.Shared;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using WEngine.MVP;
using ZBase.UnityScreenNavigator.Core;
using ZBase.UnityScreenNavigator.Core.Activities;
using ZBase.UnityScreenNavigator.Core.Modals;
using ZBase.UnityScreenNavigator.Core.Screens;
using ZBase.UnityScreenNavigator.Core.Windows;
using System;
using GameCore.Presentation;
using GameCore.Presentation.GamePlay;

namespace GameCore.Presentaion.Shared
{
    public class TransitionService : ICloseTransition
    {
        private IObjectResolver _resolver;
        private ModalContainer _modalContainer;
        private ActivityContainer _activityContainer;
        private ActivityContainer _loadingContainer ;
        private ScreenContainer _screenContainer;
        public bool IsModalInTransition => _modalContainer.IsInTransition;
        
        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        
        public void FindContainer(IWindowContainerManager containerManager)
        {
            _modalContainer = containerManager.Find<ModalContainer>();
            _activityContainer = containerManager.Find<ActivityContainer>("ActivityContainer");
            _loadingContainer = containerManager.Find<ActivityContainer>("LoadingContainer");
            _screenContainer = containerManager.Find<ScreenContainer>();
            FixMask(_modalContainer, _activityContainer, _loadingContainer, _screenContainer);
        }
        
        private void FixMask(params WindowContainerBase[] containerBases)
        {
            foreach (var containerBase in containerBases)
            {
                var mask2D = containerBase.GetComponent<RectMask2D>();
                mask2D.padding = Vector4.one * -1;
            }
        }

        public UniTask ClosePopup()
        {
            var modalsCount = _modalContainer.Modals.Count;
            return modalsCount > 0 ? _modalContainer.PopAsync(true) : UniTask.CompletedTask;
        }

        public void CloseAllPopup()
        {
            var modalsCount = _modalContainer.Modals.Count;
            for (var i = 0; i < modalsCount; i++)
            {
                _modalContainer.Pop(false);
            }
        }

        public UniTask<bool> BackToPreviousScreen()
        {
            return _screenContainer.Screens.Count > 1
                ? _screenContainer.PopAsync(true)
                    .ContinueWith(() => true)
                : UniTask.FromResult(false);
        }
         private UniTask<T> ShowScreenPresenterAsync<T, TView, TState>(string key, Func<TView, T> createFunc,
            bool isStack = true, bool isPooling = false)
            where T : ScreenPresenter<TView, TState>
            where TView : Screen<TState>
            where TState : ViewState, new()
        {
            var tcs = new UniTaskCompletionSource<T>();
            var options = new ScreenOptions(key,
                onLoaded: (view, args) =>
                {
                    var presenter = createFunc((TView)view);
                    _resolver.Inject(presenter);
                    presenter.Initialize();
                    tcs.TrySetResult(presenter);
                }, stack: isStack,
                poolingPolicy: isPooling ? PoolingPolicy.EnablePooling : PoolingPolicy.DisablePooling);
            _screenContainer.Push<TView>(options);
            return tcs.Task;
        }

        private UniTask<T> ShowModalPresenterAsync<T, TView, TState>(string key, Func<TView, T> createFunc)
            where T : ModalPresenter<TView, TState>
            where TView : Modal<TState>
            where TState : ViewState, new()
        {
            var tcs = new UniTaskCompletionSource<T>();
            var options = new ModalOptions(key, onLoaded: (view, args) =>
            {
                var presenter = createFunc((TView)view);
                _resolver.Inject(presenter);
                presenter.Initialize();
                tcs.TrySetResult(presenter);
            });
            _modalContainer.Push<TView>(options);
            return tcs.Task;
        }

        private UniTask<T> ShowActivityPresenterAsync<T, TView, TState>(string key, Func<TView, T> createFunc)
            where T : ActivityPresenter<TView, TState>
            where TView : Activity<TState>
            where TState : ViewState, new()
        {
            var tcs = new UniTaskCompletionSource<T>();
            var options = new ActivityOptions(key, onLoaded: (view, args) =>
            {
                var presenter = createFunc((TView)view);
                _resolver.Inject(presenter);
                presenter.Initialize();
                tcs.TrySetResult(presenter);
            });
            _activityContainer.Show<TView>(options);
            return tcs.Task;
        }

        public async UniTask<TestPresenter> ShowTestModal()
        {
            var presentor = await ShowModalPresenterAsync<TestPresenter, TestModal, TestViewState>(
                "TestModal",
                modal => new TestPresenter(modal));
            return presentor;
        }

        public async UniTask<GamePlayPresenter> ShowGamePlayScreen()
        {
            var presenter = await ShowScreenPresenterAsync<GamePlayPresenter, GamePlayScreen, GamePlayViewState>(
                "GamePlayScreen",
                screen => new GamePlayPresenter(screen), false);
            return presenter;
        }

    }

}
