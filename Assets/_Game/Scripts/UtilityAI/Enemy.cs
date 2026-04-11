using System.Collections.Generic;
using System.Linq;
using CoreGame;
using GameCore.Utility;
using Terramorphers;

using UnityEngine;
using UtilityAI.State;
using VitalRouter;

using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UtilityAI.AIActions;
using VContainer;

namespace UtilityAI
{
    public class Enemy : TerramorphersEntity
    {
        [OdinSerialize, ShowInInspector, ReadOnly, TabGroup("Debug")]
        public Dictionary<string, float> ActionEvaluationDebug = new();
        
        private readonly int idleAnimHash = Animator.StringToHash("Idle");
        private readonly int attackAnimHash = Animator.StringToHash("Attacking");
        private readonly int dyingAnimHash = Animator.StringToHash("Dying");
        private readonly int idleBlinkAnimHah = Animator.StringToHash("IdleBlink");
        private readonly int hurtAnimHash = Animator.StringToHash("Hurt");
        private readonly int walkAnimHash = Animator.StringToHash("Walking");
        private readonly int tauntAnimHash = Animator.StringToHash("Taunt");
        protected EntityManager _entityManager;
        protected BoardManager _boardManager;
        protected ICommandPublisher _publisher;
        protected SkillManager _skillManager;
      
        public EntityManager EntityManager => _entityManager;
        public BoardManager BoardManager => _boardManager;

        public ICommandPublisher Publisher => _publisher;

        public SkillManager SkillManager => _skillManager;
      
        protected ThinkingState _thinkingState;
        protected MoveState _moveState;
        protected UseSkillState _useSkillState;

        public ThinkingState ThinkingState => _thinkingState;

        public UseSkillState UseSkillState => _useSkillState;
        [HideInInspector] public IReadOnlyList<AIAction> AIActions;

        public MoveState MoveState => _moveState;
        private EnemyMetadata _enemyMetadata;

        public EnemyMetadata EnemyMetadata => _enemyMetadata;


        [Inject]
        public void Construct(BoardManager boardManager,
            ICommandPublisher publisher, ICommandSubscribable commandSubscribable,
            EntityManager entityManager, SkillManager skillManager)
        {
            _boardManager = boardManager;
            _publisher = publisher;
            _entityManager = entityManager;
            _skillManager = skillManager;
        }

        protected override void Awake()
        {
            base.Awake();
           
            _moveState = new MoveState(this,walkAnimHash);
            _idleState = new IdleState(this,
                new List<int>() { idleAnimHash, idleBlinkAnimHah },
                new List<List<float>>()
                {
                    new List<float>(){.7f, .3f},
                    new List<float>(){.95f,.05f}
                    
                });
            AddState(_idleState);
            
            AddState(_moveState);
            ChangeState(_idleState);
        }

        public override void Init(EntityMetadata metadata, int teamID)
        {
            base.Init(metadata, teamID);
            if (metadata is not EnemyMetadata enemyMetadata)
            {
                Debug.Log($"[Test] type of enemy meta data is not correct");
                return;
            }
           

            _enemyMetadata = enemyMetadata;
            _thinkingState = new ThinkingState(this,idleAnimHash, enemyMetadata);
            _useSkillState = new UseSkillState(this, enemyMetadata.SkillConsiderationDatas);
           
            AddState(_useSkillState);
            AddState(_thinkingState);

          
            Context.SetData(BlackBoardConstant.OWNER_KEY, this);
            Context.SetData(BlackBoardConstant.REMAIN_STAMINA_KEY, statsSystem.Stats.Stamina);
            Context.SetData(BlackBoardConstant.REMAIN_MANA_KEY, statsSystem.Stats.Mana);
          


        }

        public override void OnEnter()
        {
            base.OnEnter();
           
            ChangeState(_thinkingState);
        }

        public virtual void UpdateContext()
        {
            
        }
        protected void Update()
        {
            UpdateContext();
            _stateMachine.Update();
        }


        public override void OnUpdate()
        {
            
        }

        public override void OnExit()
        {
            Context.SetData(BlackBoardConstant.REMAIN_STAMINA_KEY, statsSystem.Stats.Stamina);
            Context.SetData(BlackBoardConstant.REMAIN_MANA_KEY, statsSystem.Stats.Mana);
            ChangeState(_idleState);
            statsSystem.Update();
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
            //context.CurrentTile = tile;
        }
        public override void SetDirection(Vector3 direction)
        {
           
            if (direction.x > 0 && Model.localScale.x < 0) Model.localScale = Model.localScale.Set(x: Model.localScale.x * -1);
            else if (direction.x < 0 && Model.localScale.x > 0)
            {

                Model.localScale = Model.localScale.Set(x: Model.localScale.x * -1);
            }
        }

        public override bool IsDead()
        {
            return false;
        }
    }
}