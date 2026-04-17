using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameCore.Commands;
using GameCore.Utility;
using GameCore.Utility.Shape;
using Terramorphers.Command;
using UnityEngine;
using VitalRouter;

namespace Terramorphers
{
    public class EntityManager
    {
        private List<TerramorphersEntity> _entities = new();
        private int currentEntityID;
        private EntityFactory _entityFactory;
        private List<IDisposable> bags = new();
        private ICommandSubscribable _subscribable;
        private ICommandPublisher _publisher;
        private int currentRound;
        private Dictionary<int, int> teamMembersDict = new();
        public Player Player;

        public EntityManager(EntityFactory entityFactory, ICommandSubscribable subscribable, ICommandPublisher publisher)
        {
            _entityFactory = entityFactory;
            _subscribable = subscribable;
            _publisher = publisher;
            currentEntityID = -1;
        }

        public void ClearEntity()
        {
            currentEntityID = -1;
            foreach (var entity in _entities)
            {
                if (entity != null)
                {
                    entity.OnExit();
                    entity.Dispose();
                    _entityFactory.Despawn(entity);
                }
            }
            _entities.Clear();
            teamMembersDict.Clear();
            Player = null;
        }

        public async UniTask AddEntity(int entityID, ITile tile, int teamID)
        {
            TerramorphersEntity entity = await _entityFactory.Create(entityID, teamID, tile);
            teamMembersDict.TryAdd(teamID, 0);
            teamMembersDict[teamID]++;
            if (entity is Player player) Player = player;
            if (entity == null)
            {
                Debug.Log($"[EntityManager] can not add entity {entityID}");
                return;
            }
            if (currentEntityID == 0)
            {
                _entities.Insert(1, entity);
            }
            else
            {
                _entities.Add(entity);
            }
        }

        public void RemoveEntity(TerramorphersEntity entity)
        {
            if (currentEntityID >= 0 && currentEntityID < _entities.Count && _entities[currentEntityID] == entity)
            {
                _entities[currentEntityID].OnExit();
            }

            _entities.Remove(entity);
            _entityFactory.Despawn(entity);
            teamMembersDict[entity.TeamID]--;
            if (teamMembersDict[entity.TeamID] == 0) teamMembersDict.Remove(entity.TeamID);
            if(teamMembersDict.Count <= 1) CheckEndGame();
        }
        

        private void CheckEndGame()
        {
            if (teamMembersDict.Count == 0)
            {
                _publisher.PublishAsync(new ChangeGameStateTypeCommand(EGameStateType.WinState));
            }
            else if (teamMembersDict.Keys.First() == Player.TeamID)
            {
                _publisher.PublishAsync(new ChangeGameStateTypeCommand(EGameStateType.WinState));
            }
            else _publisher.PublishAsync(new ChangeGameStateTypeCommand(EGameStateType.LoseState));
        }
        public void ResetEntityID() => currentEntityID = 0;

        public void OnEnter()
        {
            currentRound = 0;
            _publisher.PublishAsync(new IncreaseRoundCommand() { NewRound = currentRound });
            if (currentEntityID >= 0 &&
                currentEntityID < _entities.Count &&
                _entities[currentEntityID] != null) _entities[currentEntityID].OnEnter();

            bags.Add(_subscribable.Subscribe<EndEntityTurnCommand>(EndCurrentEntityTurn));
        }

        public void OnUpdate()
        {
            if (currentEntityID >= 0 &&
                currentEntityID < _entities.Count &&
                _entities[currentEntityID] != null) _entities[currentEntityID].OnUpdate();
        }

        public void OnExit()
        {
            if (currentEntityID >= 0 &&
                currentEntityID < _entities.Count &&
                _entities[currentEntityID] != null) _entities[currentEntityID].OnExit();

            foreach (var bag in bags) bag?.Dispose();
        }

        private void EndCurrentEntityTurn(EndEntityTurnCommand command, PublishContext context)
        {
            if (currentEntityID >= 0 &&
                currentEntityID < _entities.Count &&
                _entities[currentEntityID] != null)
            {
                _publisher.PublishAsync(new EntityTileDistCommand()
                {
                    Tile = _entities[currentEntityID].CurrentTile,
                    Name = _entities[currentEntityID].Name
                });
                _entities[currentEntityID].OnExit();
            }

            currentEntityID++;
            if (currentEntityID >= _entities.Count)
            {
                currentEntityID = 0;
                currentRound++;
                _publisher.PublishAsync(new IncreaseRoundCommand() { NewRound = currentRound });
            }


            if (_entities[currentEntityID] != null) _entities[currentEntityID].OnEnter();
        }

        public List<TerramorphersEntity> GetEnemies(TerramorphersEntity owner)
        {
            return null;
        }

        public List<TerramorphersEntity> GetEntitiesWithTeamID(int teamID) => _entities.Where(t => t.TeamID == teamID).ToList();

        public TerramorphersEntity GetClosestEntityWithTeamID(int teamID, ITile center)
        {
            List<TerramorphersEntity> entities = GetEntitiesWithTeamID(teamID);
            if (entities == null || entities.Count == 0) return null;
            return entities
                .MinBy(e => Cube.Distance(e.CurrentTile.Index, center.Index));
        }

        public List<TerramorphersEntity> GetEntitiesWithDiffTeamID(int teamID) => _entities.Where(t => t.TeamID != teamID).ToList();

        public TerramorphersEntity GetClosestEntityWithDiffTeamID(int teamID, ITile center)
        {
            List<TerramorphersEntity> entities = GetEntitiesWithDiffTeamID(teamID);
            if (entities == null || entities.Count == 0) return null;
            return entities
                .MinBy(e => Cube.Distance(e.CurrentTile.Index, center.Index));
        }
    }
}