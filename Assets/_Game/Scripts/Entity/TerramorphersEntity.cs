
using System;
using CoreGame;
using GameCore.Domain.Skill;
using GameCore.Domain.Stats;
using GameCore.Usecase.Quest;
using GameCore.Utility;
using R3;
using Sirenix.OdinInspector;
using Terramorphers.Command;
using Terramorphers.Skill;
using Terramorphers.States;

using UnityEngine;
using UtilityAI;
using UtilityAI.DataCache;
using VContainer;
using VitalRouter;

namespace Terramorphers
{
    public abstract class TerramorphersEntity : Entity, IDisposable
    {
        [SerializeField, TabGroup("General")] public float MoveSpeed = .5f;
        [TabGroup("Components")]
        protected StatsSystem statsSystem;
        [TabGroup("Debug")] public Context Context;
        protected IState _idleState;
        protected IState _hurtState;
        protected IState _deadState;
      
        protected EntityDataCache dataCache;
        protected EntityDerivedDataCalculator derivedDataCalculator;
        [Inject] protected BoardManager _boardManager;
        [Inject] protected EntityManager _entityManager;
        [Inject] protected QuestUseCase _questUseCase;

        [Inject] protected SkillSystem _skillSystem;
        [Inject] protected ICommandPublisher _publisher;

        public ICommandPublisher Publisher => _publisher;


        public SkillSystem SkillSystem => _skillSystem;
        protected DisposableBag _bag;

        public EntityManager EntityManager => _entityManager;

        public BoardManager BoardManager => _boardManager;
        public EntityDataCache DataCache => dataCache;

        public QuestUseCase QuestUseCase => _questUseCase;

        public EntityDerivedDataCalculator DerivedDataCalculator => derivedDataCalculator;

       
      
        public IState IdleState => _idleState;

        public IState HurtState => _hurtState;

        public IState DeadState => _deadState;
        protected ITile currentTile;
        [HideInInspector] public Action OnInitialized;
     
        private void OnValidate()
        {
            MoveSpeed = .5f;
        }

        public virtual void PreEnter()
        {
          
            _skillSystem.OnEnter();
            statsSystem.HandeEvent(this,EEffectTriggerType.EnterTurn);
            ResetDataCache();
        }
        public virtual void OnEnter()
        {
            
            
        }
        public abstract void OnUpdate();

        public virtual void OnExit()
        {
            
            statsSystem.Update();
            dataCache.RemainMana.Value = statsSystem.Stats.Mana;
            dataCache.RemainStamina.Value = statsSystem.Stats.Stamina;
        }
        public abstract void SetTile(ITile tile);
        public abstract bool IsDead();
        public ITile CurrentTile => currentTile;
        protected int _teamID;

        public int TeamID => _teamID;

        public StatsSystem StatsSystem => statsSystem;
        [HideInInspector] public string Name;
        public int ID { get; protected set; }

     
        public virtual void Init(int id, EntityMetadata metadata, int teamID, ITile tile)
        {
            SetTile(tile);
            ID = id;
            statsSystem = new StatsSystem(metadata.EntityStats);
            Context = new Context();
            dataCache = new EntityDataCache();
            derivedDataCalculator = new EntityDerivedDataCalculator(this);
            Name = metadata.Addressable;
            _teamID = teamID;
            
          
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

        public void Heal(int healAmount)
        {
            healAmount = statsSystem.Stats.GetHealAmount(healAmount);
            dataCache.RemainHP.Value = Mathf.Min(dataCache.RemainHP.Value + healAmount, statsSystem.Stats.MaxHP);
        }
        public virtual void SetDirection(Vector3 direction)
        {

            if (direction.x > 0 && Model.localScale.x < 0)
            {
                var localScale = Model.localScale;
                localScale = localScale.Set(x: localScale.x * -1);
                Model.localScale = localScale;
            }
            else if (direction.x < 0 && Model.localScale.x > 0)
            {
                var localScale = Model.localScale;
                localScale = localScale.Set(x: localScale.x * -1);
                Model.localScale = localScale;
            }
        }

        private void OnDestroy()
        {
            derivedDataCalculator.Dispose();
        }

        public void Dispose()
        {
            derivedDataCalculator?.Dispose();
            _bag.Dispose();
        }

      
    }
}