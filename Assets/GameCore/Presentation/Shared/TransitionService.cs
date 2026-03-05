using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Presentation.Shared;
using UnityEngine;
using VContainer;
using ZBase.UnityScreenNavigator.Core.Activities;
using ZBase.UnityScreenNavigator.Core.Modals;
using ZBase.UnityScreenNavigator.Core.Screens;

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
        
        // public void FindContainer(IWindowContainerManager containerManager)
        // {
        //     _modalContainer = containerManager.Find<ModalContainer>();
        //     _activityContainer = containerManager.Find<ActivityContainer>("ActivityContainer");
        //     _loadingContainer = containerManager.Find<ActivityContainer>("LoadingContainer");
        //     _screenContainer = containerManager.Find<ScreenContainer>();
        //     FixMask(_modalContainer, _activityContainer, _loadingContainer, _screenContainer);
        // }
        //
        // private void FixMask(params WindowContainerBase[] containerBases)
        // {
        //     foreach (var containerBase in containerBases)
        //     {
        //         var mask2D = containerBase.GetComponent<RectMask2D>();
        //         mask2D.padding = Vector4.one * -1;
        //     }
        // }

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

    }

}
