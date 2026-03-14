using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Terramorphers
{
    public class EntityManager
    {
        private LinkedList<TerramorphersEntity> _entities = new();
        private LinkedListNode<TerramorphersEntity> _currentEntities;
        private EntityFactory _entityFactory;

        public EntityManager(EntityFactory entityFactory)
        {
            _entityFactory = entityFactory;
        }

        public async UniTask AddEntity(int entityID, ITile tile)
        {
            TerramorphersEntity entity = await _entityFactory.Create(entityID);
            if (entity == null)
            {
                Debug.Log($"[EntityManager] can not add entity {entityID}");
                return;
            }
            entity.SetTile(tile);

            if (_currentEntities == null)
            {
                _entities.AddFirst(entity);
            }
            else _entities.AddAfter(_currentEntities, entity);
        }
        
        public void OnEnter()
        {
            _currentEntities = _entities.First;
            if(_currentEntities != null) _currentEntities.Value.OnEnter();
        }

        public void OnUpdate()
        {
            if(_currentEntities != null) _currentEntities.Value.OnUpdate();
        }

        public void OnExit()
        {
            if(_currentEntities != null) _currentEntities.Value.OnExit();
        }

    }
}