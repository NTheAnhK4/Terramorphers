using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Commands;
using Sirenix.Utilities;
using UnityEngine;
using VitalRouter;

namespace Terramorphers
{
    public class EntityManager
    {
        private LinkedList<TerramorphersEntity> _entities = new();
        private LinkedListNode<TerramorphersEntity> _currentEntity;
        private EntityFactory _entityFactory;
        private List<IDisposable> bags = new();
        private ICommandSubscribable _subscribable;
        private ICommandPublisher _publisher;
        private int currentRound;

        public EntityManager(EntityFactory entityFactory, ICommandSubscribable subscribable, ICommandPublisher publisher)
        {
            _entityFactory = entityFactory;
            _subscribable = subscribable;
            _publisher = publisher;
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

            if (_currentEntity == null)
            {
                _entities.AddFirst(entity);
            }
            else _entities.AddAfter(_currentEntity, entity);
        }
        
        public void OnEnter()
        {
            currentRound = 1;
            _currentEntity = _entities.First;
            if(_currentEntity != null) _currentEntity.Value.OnEnter();
            bags.Add(_subscribable.Subscribe<EndEntityTurnCommand>(EndCurrentEntityTurn));
        }

        public void OnUpdate()
        {
            if(_currentEntity != null) _currentEntity.Value.OnUpdate();
        }

        public void OnExit()
        {
            if(_currentEntity != null) _currentEntity.Value.OnExit();
            foreach(var bag in bags) bag?.Dispose();
        }

        private void EndCurrentEntityTurn(EndEntityTurnCommand command, PublishContext context)
        {
            _currentEntity.Value.OnExit();
            if (_currentEntity.Next == null)
            {
                _currentEntity = _entities.First;
                currentRound++;
                _publisher.PublishAsync(new IncreaseRoundCommand() {NewRound = currentRound});
            }
            else _currentEntity = _currentEntity.Next;
           
            _currentEntity.Value.OnEnter();
        }

    }
}