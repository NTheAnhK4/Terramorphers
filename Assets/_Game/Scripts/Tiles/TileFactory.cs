using CoreGame;
using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace Terramorphers
{
    public class TileFactory : ITileFactory
    {
        private IObjectResolver _resolver;
        private TileDatabase _tileDatabase;
        private Transform _parent;
        public TileFactory(TileDatabase tileDatabase, IObjectResolver resolver)
        {
            _tileDatabase = tileDatabase;
            _resolver = resolver;
            
        }
        public async UniTask<ITile> CreateTile(ETileType type)
        {
            
            var tileMetadata = _tileDatabase.GetByType(type);
           
          
            var tilePrefab = await Addressables.LoadAssetAsync<GameObject>(tileMetadata.Addresable);
            if (tilePrefab == null)
            {
                Debug.LogError($"[Tile] Can not find addressable for {tileMetadata.Addresable}");
                return null;
            }

            var tile = PoolingManager.Spawn(tilePrefab,_parent).GetComponent<ITile>();
            _resolver.Inject(tile);
            return tile;
        }

        public void SetParent(Transform parent)
        {
            _parent = parent;
        }
    }
}