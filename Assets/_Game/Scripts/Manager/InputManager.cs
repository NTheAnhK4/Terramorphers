
using Terramorphers.Command;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace Terramorphers
{
    public class InputManager
    {
        private ICommandPublisher _publisher;
        private Camera mainCamera;
        [Inject]
        public void Constructor(ICommandPublisher publisher)
        {
            _publisher = publisher;
        }

      

        public void OnEnter()
        {
            mainCamera = Camera.main;
        }

        public void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ITile tile = GetTile();
                if (tile == null) return;
                _publisher.PublishAsync(new SelectTileCommand() { SelectedTile = tile });
            }
        }
        

        ITile GetTile()
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider == null) return null;
            return hit.collider.gameObject.GetComponentInParent<ITile>();
            
        }
       

       
    }
}

