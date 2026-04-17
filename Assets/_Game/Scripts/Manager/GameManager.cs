using CoreGame;
using GameCore.Commands;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace Terramorphers
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private ICommandPublisher _publisher;
        public GameFSM GameFSM { get; set; }
   
        
        
    }
}