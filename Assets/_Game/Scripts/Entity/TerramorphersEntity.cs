
using System;
using CoreGame;
using GameCore.Domain.Skill;
using GameCore.Utility;
using Sirenix.OdinInspector;
using Terramorphers.States;
using Terramorphers.Stats;
using UnityEngine;
using UtilityAI;
using UtilityAI.DataCache;
using VContainer;

namespace Terramorphers
{
    public abstract class TerramorphersEntity : Entity
    {
        [SerializeField, TabGroup("General")] public float MoveSpeed = .5f;
        [SerializeField, TabGroup("Components")]
        protected StatsSystem statsSystem;
        [TabGroup("Debug")] public Context Context;
        protected IState _idleState;
        protected IState _hurtState;
        protected IState _deadState;
      
        protected EntityDataCache dataCache;
        protected EntityDerivedDataCalculator derivedDataCalculator;
        [Inject] protected BoardManager _boardManager;

        public BoardManager BoardManager => _boardManager;
        public EntityDataCache DataCache => dataCache;

        public EntityDerivedDataCalculator DerivedDataCalculator => derivedDataCalculator;

       

        public IState IdleState => _idleState;

        public IState HurtState => _hurtState;

        public IState DeadState => _deadState;
        protected ITile currentTile;

        private void OnValidate()
        {
            MoveSpeed = .5f;
        }

        public virtual void OnEnter()
        {
            ResetDataCache();
        }
        public abstract void OnUpdate();
        public abstract void OnExit();
        public abstract void SetTile(ITile tile);
        public abstract bool IsDead();
        public ITile CurrentTile => currentTile;
        protected int _teamID;

        public int TeamID => _teamID;

        public StatsSystem StatsSystem => statsSystem;
        [HideInInspector] public string Name;

        public virtual void Init(EntityMetadata metadata, int teamID)
        {
            statsSystem = new StatsSystem(metadata.EntityStats);
            Context = new Context();
            dataCache = new EntityDataCache();
            derivedDataCalculator = new EntityDerivedDataCalculator(this);
            Name = metadata.Addressable;
            _teamID = teamID;
           
            
        
         
            ResetDataCache();
            dataCache.RemainHP.Value = statsSystem.Stats.MaxHP;
        }

        protected void ResetDataCache()
        {
            dataCache.RemainMana.Value = statsSystem.Stats.Mana;
            dataCache.RemainStamina.Value = statsSystem.Stats.Stamina;
        }

        public void TakeDamage(int damage, EAttackType attackType)
        {
            ChangeState(_hurtState, () => new HurtStateData(){Damage = damage,AttackType = attackType});
        }
        public virtual void SetDirection(Vector3 direction)
        {
           
            if (direction.x > 0 && Model.localScale.x < 0) Model.localScale = Model.localScale.Set(x: Model.localScale.x * -1);
            else if (direction.x < 0 && Model.localScale.x > 0)
            {

                Model.localScale = Model.localScale.Set(x: Model.localScale.x * -1);
            }
        }

        private void OnDestroy()
        {
            derivedDataCalculator.Dispose();
        }
    }
}