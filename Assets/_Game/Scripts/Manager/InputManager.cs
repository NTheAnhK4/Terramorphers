

using System.Collections.Generic;
using Terramorphers.Command;
using UnityEngine;

using VContainer;
using VitalRouter;
using ZBase.UnityScreenNavigator.Core.Views;

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
        public bool IsObjectClickable { get; set; } = false;

        #region Hold Variable

        private Dictionary<Collider2D, IInfoProvider> infoProviderCache = new();
        private float holdTime = 0f;
        private float holdThreshold = 0.15f;

        private bool isHolding = false;
        private bool isHoldTriggered = false;
        private IInfoProvider currentInfoProvider;

      

        #endregion
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

        // private IInfoProvider GetInfoProvider()
        // { 
        //     Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //     var layer = (1 << LayerMask.NameToLayer(ENTITY_LAYER)) |
        //                 (1 << LayerMask.NameToLayer(TILE_LAYER));
        //     RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero,Mathf.Infinity,layer);
        //     if (hit.collider == null) return null;
        //  
        //     if (!infoProviderCache.TryGetValue(hit.collider, out var infoProvider))
        //     {
        //         infoProvider = hit.collider.GetComponentInParent<IInfoProvider>();
        //         if (infoProvider != null) infoProviderCache[hit.collider] = infoProvider;
        //     }
        //
        //     return infoProvider;
        //
        // }
        private Dictionary<Collider2D, (IInfoProvider provider, SpriteRenderer sprite)> cache 
            = new();

        private IInfoProvider GetInfoProvider()
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            int entityLayer = LayerMask.NameToLayer(ENTITY_LAYER);
            int tileLayer = LayerMask.NameToLayer(TILE_LAYER);

            int layerMask = (1 << entityLayer) | (1 << tileLayer);

            var hits = Physics2D.OverlapPointAll(mousePos, layerMask);
            if (hits == null || hits.Length == 0) return null;

            Collider2D bestCollider = null;
            int bestPriority = int.MinValue;

            foreach (var col in hits)
            {
                if (!cache.TryGetValue(col, out var data))
                {
                    var provider = col.GetComponentInParent<IInfoProvider>();
                    var sprite = col.GetComponentInParent<SpriteRenderer>();

                  
                    if (provider != null)
                    {
                        data = (provider, sprite);
                        cache[col] = data;
                    }
                    else
                    {
                        continue;
                    }
                }

                int priority = 0;

               
                if (col.gameObject.layer == entityLayer)
                    priority += 10000;

              
                if (data.sprite != null)
                    priority += data.sprite.sortingOrder;

                if (priority > bestPriority)
                {
                    bestPriority = priority;
                    bestCollider = col;
                }
            }

            if (bestCollider == null) return null;

            return cache[bestCollider].provider;
        }
       

        public void OnUpdate()
        {
            if (isStopInput) return;
            if (Input.GetMouseButtonDown(0))
            {
               
                holdTime = 0f;
                isHolding = true;
                isHoldTriggered = false;
                currentInfoProvider = GetInfoProvider();
            }

            if (Input.GetMouseButton(0) && isHolding && currentInfoProvider != null)
            {
                holdTime += Time.deltaTime;
                if (!isHoldTriggered && holdTime >= holdThreshold)
                {
                    isHoldTriggered = true;
                    currentInfoProvider.IsShowInfo.Value = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && isHolding)
            {
                if (currentInfoProvider != null) currentInfoProvider.IsShowInfo.Value = false;
                if(!isHoldTriggered) HandleClick();
                isHoldTriggered = false;
            }
        }

        private void HandleClick()
        {
            Debug.Log($"[Test] handle click with {IsObjectClickable}");
            if (!IsObjectClickable) return;
            ITile tile = GetTile();
                
            if (tile == null) return;
               
            _publisher.PublishAsync(new SelectTileCommand() { SelectedTile = tile });
        }


        ITile GetTile()
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero,Mathf.Infinity,targetLayer);
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

