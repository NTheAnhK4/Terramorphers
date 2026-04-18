

using System.Collections.Generic;
using Terramorphers.Command;
using UnityEngine;

using VContainer;
using VitalRouter;

namespace Terramorphers
{
    public class InputManager
    {
        public static string TILE_LAYER = "Tile";
        public static string ENTITY_LAYER = "Entity";
        public static string UI_Layer = "UI";
        public static string IGNORE_RAYCAST = "Ignore Raycast";
        private ICommandPublisher _publisher;
        private Camera mainCamera;
        private LayerMask targetLayer;
        private Dictionary<Collider2D, ITile> tileCache = new();
        private Dictionary<Collider2D, TerramorphersEntity> entityCache = new();
        private bool isStopInput = false;
        [Inject]
        public void Constructor(ICommandPublisher publisher)
        {
            _publisher = publisher;
        }

        public void StopInput(bool isStop) => isStopInput = isStop;

        public void SetLayer(string layerName)
        {
            targetLayer = (1 << LayerMask.NameToLayer(UI_Layer)) |
                          (1 << LayerMask.NameToLayer(layerName));
        }

     
     

        public void OnEnter()
        {
            if(mainCamera == null) mainCamera = Camera.main;
        }
       

        public void OnUpdate()
        {
            if (isStopInput) return;
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
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero,targetLayer);
            if(hit.collider == null) return null;
           
            if (tileCache.TryGetValue(hit.collider, out var tile)) return tile;
            tile = hit.collider.GetComponentInParent<ITile>();
            if (tile != null)
            {
                tileCache[hit.collider] = tile;
                return tile;
            }
            if ((targetLayer & (1 << LayerMask.NameToLayer(ENTITY_LAYER))) != 0)
            {
                TerramorphersEntity entity;
                if (entityCache.TryGetValue(hit.collider, out var value1)) entity = value1;
                else
                {
                    entity = hit.collider.GetComponentInParent<TerramorphersEntity>();
                    if (entity != null) entityCache[hit.collider] = entity;
                    else return null;
                }
                tile = entity.CurrentTile;
                return tile;
            }
           
            return null;
        }
        
       
    }
}

