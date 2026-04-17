using Cysharp.Threading.Tasks;
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
using GameCore.Domain.Level;
using GameCore.Presentation.ChooseStage;
using GameCore.Presentation.GamePlay;
using GameCore.Presentation.Loading;
using GameCore.Presentation.Lobby;
using GameCore.Presentation.LoseGame;
using GameCore.Presentation.WinGame;
using R3;

namespace GameCore.Presentation.Shared
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

        public async UniTask<LobbyPresenter> ShowLobbyScreen()
        {
            var presenter = await ShowScreenPresenterAsync<LobbyPresenter, LobbyScreen, LobbyViewState>(
                "LobbyScreen",
                screen => new LobbyPresenter(screen), false);
            return presenter;
        }

        public async UniTask<ChooseStagePresenter> ShowChooseStageModal(int levelID, LevelMetadata levelMetadata)
        {
            var presenter = await ShowModalPresenterAsync<ChooseStagePresenter, ChooseStageModal, ChooseStageViewState>(
                "ChooseStageModal",
                modal => new ChooseStagePresenter(modal,levelID, levelMetadata));
            return presenter;
        }

        public async UniTask<LoadingViewPresenter> ShowLoadingView(bool isShow, ReactiveProperty<float> progress, Action onFinishLoading)
        {
            if (isShow)
            {
                var presenter = await ShowActivityPresenterAsync<LoadingViewPresenter, LoadingView, LoadingViewState>(
                    "LoadingView",
                    view => new LoadingViewPresenter(view,progress, onFinishLoading));
                return presenter;
            }
            else
            {
                if (_activityContainer.TryGet("LoadingView", out var viewRef))
                    await _activityContainer.HideAsync("LoadingView");
                return null;
            }
        }

        public async UniTask<WinGamePresenter> ShowWinGameModal()
        {
            var presentor = await ShowModalPresenterAsync<WinGamePresenter, WinGameModal, WinGameViewState>(
                "WinGameModal",
                modal => new WinGamePresenter(modal));
            return presentor;
        }

        public async UniTask<LoseGamePresentor> ShowLoseGameModal()
        {
            var presentor = await ShowModalPresenterAsync<LoseGamePresentor, LoseGameModal, LoseGameViewState>(
                "LoseGameModal",
                modal => new LoseGamePresentor(modal));
            return presentor;
        }
    }

}
