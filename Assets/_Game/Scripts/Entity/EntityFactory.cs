    using CoreGame;
    using Cysharp.Threading.Tasks;
    using GameCore.Domain.Entity;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using VContainer;

    namespace Terramorphers
    {
        public class EntityFactory
        {
            private IEntityRepository _entityRepository;
            private IEntityDatabase _entityDatabase;
           
            private IObjectResolver _resolver;

            public EntityFactory(IObjectResolver resolver,IEntityRepository entityRepository)
            {
                _entityRepository = entityRepository;
                _resolver = resolver;
            }
            public async UniTask<TerramorphersEntity> Create(int entityID, int entityTeamID, ITile tile)
            {
                if (_entityDatabase == null) _entityDatabase = _entityRepository.Get();
                var entityMetadata = _entityDatabase.GetByType(entityID) as EntityMetadata;
                if (entityMetadata != null)
                {
                    string address = entityMetadata.Addressable;
                    var handle = Addressables.LoadAssetAsync<GameObject>(address);
                    GameObject entityPrefab = await handle.Task;
                    GameObject entityGO = Object.Instantiate(entityPrefab);
                    TerramorphersEntity result = entityGO.GetComponent<TerramorphersEntity>();
                    if (result == null) return null;
                    _resolver.Inject(result);
                    result.Init(entityID,entityMetadata, entityTeamID, tile);
                    return result;
                }

                return null;
            }

            public void Despawn(TerramorphersEntity entity)
            {
                Object.Destroy(entity.gameObject);
            }
        }
    }