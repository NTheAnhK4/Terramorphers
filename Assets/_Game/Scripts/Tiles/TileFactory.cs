using CoreGame;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Tile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace Terramorphers
{
    public class TileFactory : ITileFactory
    {
        private IObjectResolver _resolver;
        private ITileRepository _tileRepository;
        private ITileDatabase _tileDatabase;
        
        private Transform _parent;
        public TileFactory(ITileRepository tileRepository, IObjectResolver resolver)
        {
            _tileRepository = tileRepository;
            _resolver = resolver;
            
        }
        public async UniTask<ITile> CreateTile(ETileType type)
        {
            if (_tileDatabase == null) _tileDatabase = _tileRepository.Get();
            var tileMetadata = _tileDatabase.GetByType(type);
           
          
            var tilePrefab = await Addressables.LoadAssetAsync<GameObject>(tileMetadata.Addresable);
            if (tilePrefab == null)
            {
                Debug.LogError($"[Tile] Can not find addressable for {tileMetadata.Addresable}");
                return null;
            }

            var tile = PoolingManager.Spawn(tilePrefab,_parent).GetComponent<ITile>();
            tile.Init(tileMetadata);
          
            _resolver.Inject(tile);
            return tile;
        }

        public void Despawn(ITile tile) => PoolingManager.Despawn(tile.Transform.gameObject);

        public void SetParent(Transform parent)
        {
            _parent = parent;
        }
    }
}