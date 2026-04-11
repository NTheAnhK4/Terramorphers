
using CoreGame;
using GameCore.Domain.Skill;
using GameCore.Utility;
using Sirenix.OdinInspector;
using Terramorphers.States;
using Terramorphers.Stats;
using UnityEngine;
using UtilityAI;

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
        protected int currentHP;

        public int CurrentHp
        {
            get => currentHP;
            set => currentHP = value;
        }

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
            Name = metadata.Addressable;
            _teamID = teamID;
            CurrentHp = statsSystem.Stats.MaxHP;
            Context = new Context();
            Context.SetData(string.Format(BlackBoardConstant.ENTITY_HEALTH_FULLNESS_RATIO, Name),1.0f);
        }

        public void TakeDamage(int damage, EAttackType attackType)
        {
            ChangeState(_hurtState, () => new HurtStateData(){Damage = damage,AttackType = attackType});
        }
        public virtual void SetDirection(Vector3 direction){}
    }
}