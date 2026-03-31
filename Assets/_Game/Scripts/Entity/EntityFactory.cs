    using CoreGame;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using VContainer;

    namespace Terramorphers
    {
        public class EntityFactory
        {
            private EntityDatabase _entityDatabase;
            private IObjectResolver _resolver;

            public EntityFactory(IObjectResolver resolver,EntityDatabase entityDatabase)
            {
                _entityDatabase = entityDatabase;
                _resolver = resolver;
            }
            public async UniTask<TerramorphersEntity> Create(int entityID, int entityTeamID) 
            {
                string address = _entityDatabase.GetByType(entityID).Addressable;
                var handle = Addressables.LoadAssetAsync<GameObject>(address);
                GameObject entityPrefab = await handle.Task;
                GameObject entityGO = PoolingManager.Spawn(entityPrefab);
                TerramorphersEntity result = entityGO.GetComponent<TerramorphersEntity>();
                if (result == null) return null;
                _resolver.Inject(result);
                result.Init(_entityDatabase.GetByType(entityID), entityTeamID);
                return result;
            }
        }
    }