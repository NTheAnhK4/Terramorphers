using GameCore.Commands;
using CoreGame;
using GameCore.Usecase.Skill;
using Sirenix.OdinInspector;
using Terramorphers.Command;
using Terramorphers.States;
using Terramorphers.States.PlayerState;
using UnityEngine;

using VContainer;
using VitalRouter;
using R3;
namespace Terramorphers
{
    public class Player : TerramorphersEntity
    {
        [TabGroup("General"), SerializeField] private Vector3 rightModalPos = new Vector3(.36f, 1.27f, 0);
        [TabGroup("General"), SerializeField] private Vector3 leftModalPos = new Vector3(-.33f, 1.27f, 0);
        #region Dependencies

        private InputManager _inputManager;
       
       
        private ICommandSubscribable _subscribable;
        private SkillUseCase _skillUseCase;
        
        #endregion

        private int idleAnimHash = Animator.StringToHash("Idle");
        private int runAnimHash = Animator.StringToHash("Run");
        private int attackAnimHash = Animator.StringToHash("Attack");
        private int hurtAnimHash = Animator.StringToHash("Hurt");
        private int deadAnimHash = Animator.StringToHash("Dead");
       

        #region State

        private PlayerSelectMoveTileState _playerSelectMoveTileState;
        private PlayerMoveState _moveState;
     
        private PlayerSelectSkillTileState _selectSkillTileState;
        private PlayerUseSkillState _useSkillState;
        #endregion

        #region Properties

      
       

        public ICommandSubscribable Subscribable => _subscribable;

        public InputManager InputManager => _inputManager;

        public PlayerSelectMoveTileState PlayerSelectMoveTileState => _playerSelectMoveTileState;

        public PlayerMoveState MoveState => _moveState;

        public PlayerSelectSkillTileState SelectSkillTileState => _selectSkillTileState;

        public PlayerUseSkillState UseSkillState => _useSkillState;
        

      

     
        
        

        #endregion

        [Inject]
        public void Construct(InputManager inputManager, 
           ICommandSubscribable subscribable
            ,SkillUseCase skillUseCase)
        {
            _inputManager = inputManager;
          
         
            _subscribable = subscribable;
            _skillUseCase = skillUseCase;
        }

        protected override void Awake()
        {
            base.Awake();
            _playerSelectMoveTileState = new PlayerSelectMoveTileState(this, idleAnimHash);
            _moveState = new PlayerMoveState(this, runAnimHash);
            _idleState = new PlayerIdleState(this, idleAnimHash);
           
            _selectSkillTileState = new PlayerSelectSkillTileState(this, idleAnimHash);
            _useSkillState = new PlayerUseSkillState(this, attackAnimHash);
            _hurtState = new HurtState(this, hurtAnimHash);
            _deadState = new DeadState(this, deadAnimHash);
            AddState(_idleState);
            AddState(_playerSelectMoveTileState);
            AddState(_selectSkillTileState);
            AddState(_moveState);
            AddState(_useSkillState);
            AddState(_hurtState);
            AddState(_deadState);
            ChangeState(_idleState);
        }

       

      

        public override void Init(int id, EntityMetadata metadata, int teamID, ITile tile)
        {
            base.Init(id, metadata, teamID, tile);
            var skillModel = _skillUseCase.GetModel();
            _skillSystem.Init(this,skillModel.CurrentSkills);
            
            
            //Register event
            dataCache.RemainStamina.Subscribe(value =>    _publisher.PublishAsync(new ChangePlayerStaminaCommand()
            {
                Stamina = value,
                MaxStamina = statsSystem.Stats.Stamina
            })).AddTo(ref _bag);
            dataCache.RemainMana.Subscribe(value => _publisher.PublishAsync(new ChangePlayerManaCommand()
            {
                Mana = value,
                MaxMana = statsSystem.Stats.Mana
            })).AddTo(ref _bag);
            
            //skill cool down
            foreach (var item in dataCache.SkillCoolDown)
            {
               
                int skillID = item.Key;
                item.Value.Subscribe(t =>
                {
                    _publisher.PublishAsync(new SkillCoolDownCommand() { SkillID = skillID, CoolDown = t });
                }).AddTo(ref _bag);
               
            }
            
            //
            ResetDataCache();
            dataCache.RemainHP.Value = statsSystem.Stats.MaxHP;
            OnInitialized?.Invoke();
        }

        public override void PreEnter()
        {
            base.PreEnter();
            _publisher.PublishAsync(new EnableEndTurnCommand() { IsEnable = true });
            _publisher.PublishAsync(new EnableSkillCommand() { IsEnable = true });
        }


        public override void OnEnter()
        {
            base.OnEnter();
           
          
            
            _inputManager.OnEnter();
           
            ChangeState(_playerSelectMoveTileState);
        }

        public override void OnUpdate()
        {
            _stateMachine.Update();
        }

        public override void OnExit()
        {
            base.OnExit();
            ChangeState(_idleState);
            _publisher.PublishAsync(new ClearSpecialTilesCommand());
            _publisher.PublishAsync(new EnableEndTurnCommand() { IsEnable = false });
            _publisher.PublishAsync(new EnableSkillCommand() { IsEnable = false });
           
        }

        public override void SetTile(ITile tile)
        {
            if (currentTile != null) currentTile.CurrentOccupant = null;
            transform.position = tile.Transform.position;
            currentTile = tile;
            if (currentTile != null) currentTile.CurrentOccupant = this;
        }

        public override void SetDirection(Vector3 direction)
        {
            if (direction.x > 0 && Model.localScale.x < 0)
            {
                var localScale = Model.localScale;
                localScale = localScale.Set(x: localScale.x * -1);
                Model.localScale = localScale;
                Model.transform.localPosition = rightModalPos;
            }
            else if (direction.x < 0 && Model.localScale.x > 0)
            {
                var localScale = Model.localScale;
                localScale = localScale.Set(x: localScale.x * -1);
                Model.localScale = localScale;
                Model.transform.localPosition = leftModalPos;
            }
        }

        public override bool IsDead()
        {
            return false;
        }
    }
}