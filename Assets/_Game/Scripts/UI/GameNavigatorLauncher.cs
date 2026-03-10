using System.Collections;
using System.Collections.Generic;
using GameCore.Presentaion.Shared;
using GameCore.Utility.Vibration;
using UnityEngine;
using VContainer;
using ZBase.UnityScreenNavigator.Core;

namespace Terramorphers
{
    public class GameNavigatorLauncher : UnityScreenNavigatorLauncher

    {
        [Inject]
        private TransitionService _transitionService;
        protected override void OnPostCreateContainers()
        {
            UnityScreenNavigatorSettings.Initialize();
            _transitionService.FindContainer(this);
        }
                protected override void OnAwake()
                {
                    VibrationUti.Init();
                }

        public TransitionService TransitionService => _transitionService;

        
    }

}
