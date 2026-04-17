
using System;
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
        public static string IGNORE_RAYCAST = "Ignore Raycast";
        private ICommandPublisher _publisher;
        private Camera mainCamera;
        private LayerMask targetLayer;
        private Dictionary<Collider2D, ITile> tileCache = new();
        private Dictionary<Collider2D, TerramorphersEntity> entityCache = new();
        [Inject]
        public void Constructor(ICommandPublisher publisher)
        {
            _publisher = publisher;
        }

        public void SetLayer(string layerName)
        {
            targetLayer = LayerMask.NameToLayer(layerName);
        }

     
     

        public void OnEnter()
        {
            if(mainCamera == null) mainCamera = Camera.main;
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
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero,targetLayer);
            if(hit.collider == null) return null;
            
            if (targetLayer == LayerMask.NameToLayer(ENTITY_LAYER))
            {
                TerramorphersEntity entity;
                if (entityCache.TryGetValue(hit.collider, out var value1)) entity = value1;
                else
                {
                    entity = hit.collider.GetComponentInParent<TerramorphersEntity>();
                    if (entity != null) entityCache[hit.collider] = entity;
                    else return null;
                }
                var tile = entity.CurrentTile;
                return tile;
            }
            else
            {
                if (tileCache.TryGetValue(hit.collider, out var tile)) return tile;
                else
                {
                    tile = hit.collider.GetComponentInParent<ITile>();
                    if (tile != null)
                    {
                        tileCache[hit.collider] = tile;
                        return tile;
                    }

                    return null;
                }
            }
        }
        
       
    }
}

