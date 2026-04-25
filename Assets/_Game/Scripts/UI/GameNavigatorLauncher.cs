using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Presentation.Shared;
using GameCore.Utility.Vibration;
using VContainer;
using VitalRouter;
using ZBase.UnityScreenNavigator.Core;

namespace Terramorphers
{
    public class GameNavigatorLauncher : UnityScreenNavigatorLauncher

    {
        [Inject] private TransitionService _transitionService;

        [Inject] private ICommandPublisher _publisher;

        protected override void OnPostCreateContainers()
        {
            UnityScreenNavigatorSettings.Initialize();
            _transitionService.FindContainer(this);
            StartFSM().Forget();
        }

        protected override void OnAwake()
        {
            VibrationUti.Init();
        }

        public TransitionService TransitionService => _transitionService;

        async UniTask StartFSM()
        {
            _publisher.PublishAsync(new ChangeGameStateTypeCommand(EGameStateType.LobbyState)).AsUniTask().Forget();
        }
    }
}