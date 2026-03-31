
using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame;
using Sirenix.OdinInspector;
using Terramorphers.Stats;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace Terramorphers
{
    public class Enemy : TerramorphersEntity
    {
        
        
        
        protected EntityManager _entityManager;
        protected BoardManager _boardManager;
        protected ICommandPublisher _publisher;
        protected ITile targetTile;
        protected Dictionary<IState, float> stateCosts = new();

        public ITile TargetTile => targetTile;
        protected float minStateCost = float.MinValue;

        public EntityManager EntityManager => _entityManager;
        public BoardManager BoardManager => _boardManager;

        public ICommandPublisher Publisher => _publisher;

        public int RemainStamina;
        public Stats.Stats Stats => statsSystem.Stats;
       

        #region Runtime Data

        public List<ITile> MovePath = new();
        

        #endregion
        [Inject]
        public void Construct(EntityManager entityManager, BoardManager boardManager, ICommandPublisher publisher)
        {
            _entityManager = entityManager;
            _boardManager = boardManager;
            _publisher = publisher;
        }

        

        

        public void SetStateCost(IState state, float value)
        {
            stateCosts[state] = value;
        }

        public void CaculateMinCost()
        {
            minStateCost = stateCosts.Values.Min();
        }

        public void ResetMinStateCost() => minStateCost = float.MinValue;

        protected override void AddState(IState newState)
        {
            base.AddState(newState);
            stateCosts[newState] = float.MaxValue;
            Any(newState, ConditionToState(newState));
        }

        public void SetStateWithMinCost(IState state)
        {
            var keys = stateCosts.Keys.ToList();

            foreach (var st in keys)
            {
                stateCosts[st] = 1;
            }

            if (stateCosts.ContainsKey(state))
            {
                stateCosts[state] = 0;
            }
        }

        protected virtual FuncPredicate ConditionToState(IState state)
        {
            
            return new FuncPredicate(() =>
            {
                if (minStateCost < 0) return false;
                return Math.Abs(stateCosts[state] - minStateCost) < 0.0001f;
            });
        }

        public override void OnEnter()
        {
           
            
        }
        public virtual void TurnToThinkingState(){}

        public override void OnUpdate()
        {
           
           
        }

        protected void Update() => _stateMachine.Update();

        public override void OnExit()
        {
            statsSystem.Update();
            RemainStamina = statsSystem.Stats.Stamina;
            
        }

        public override void SetTile(ITile tile)
        {
            if (currentTile != null)
            {
                currentTile.CurrentOccupant = null;
            }
            transform.position = tile.Transform.position;
            currentTile = tile;
            if (currentTile != null) currentTile.CurrentOccupant = this;
        }

        public override bool IsDead()
        {
            return false;
        }

        public virtual void SetDirection(Vector3 direction)
        {
           
            if (direction.x > 0 && Model.localScale.x < 0) Model.localScale = Model.localScale.Set(x: Model.localScale.x * -1);
            else if (direction.x < 0 && Model.localScale.x > 0)
            {
                
                Model.localScale = Model.localScale.Set(x: Model.localScale.x * -1);
            }
        }
    }

}
