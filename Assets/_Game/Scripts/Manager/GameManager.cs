using CoreGame;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace Terramorphers
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private ICommandPublisher _publisher;
        public GameFSM GameFSM { get; set; }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) _publisher.PublishAsync(new ChangeGameStateTypeCommand(EGameStateType.WinState));
        }
    }
}