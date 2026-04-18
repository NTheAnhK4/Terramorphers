using CoreGame;
using GameCore.Commands;
using GameCore.Presentation.GamePlay;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace Terramorphers
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private ICommandPublisher _publisher;
        public GamePlayPresenter GamePlayPresenter { get; set; }

    public GameFSM GameFSM { get; set; }
   
        
        
    }
}