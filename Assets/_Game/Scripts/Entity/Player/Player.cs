using GameCore.Commands;
using GameCore.Utility;
using Terramorphers.Command;
using Terramorphers.States;
using Terramorphers.States.PlayerState;
using UnityEngine;
using VContainer;
using VitalRouter;

namespace Terramorphers
{
    public class Player : TerramorphersEntity
    {
        #region Dependencies

        private InputManager _inputManager;
       
        private ICommandPublisher _publisher;
        private ICommandSubscribable _subscribable;
        private SkillManager _skillManager;

        #endregion

        #region Runtime Data

     
        private int remainStamina;
        private int remainMana;
        #endregion

        #region State

        private PlayerSelectMoveTileState _playerSelectMoveTileState;
        private PlayerMoveState _moveState;
     
        private PlayerSelectSkillTileState _selectSkillTileState;
        private PlayerUseSkillState _useSkillState;
        #endregion

        #region Properties

        public int RemainStamina
        {
            get => remainStamina;
            set
            {
                if (value != remainStamina)
                {
                    if (_publisher != null)
                        _publisher.PublishAsync(
                            new ChangePlayerStaminaCommand()
                            {
                                Stamina = value,
                                MaxStamina = statsSystem.Stats.Stamina
                            });
                    remainStamina = value;
                }
            }
        }

        public int RemainMana
        {
            get => remainMana;
            set
            {
                if (value != remainMana)
                {
                    remainMana = value;
                    if (_publisher != null) _publisher.PublishAsync(
                        new ChangePlayerManaCommand()
                        {
                            Mana = remainMana,
                            MaxMana = statsSystem.Stats.Mana
                        });
                }
            }
        }

        public ICommandPublisher Publisher => _publisher;

        public ICommandSubscribable Subscribable => _subscribable;

        public InputManager InputManager => _inputManager;

        public PlayerSelectMoveTileState PlayerSelectMoveTileState => _playerSelectMoveTileState;

        public PlayerMoveState MoveState => _moveState;

        public PlayerSelectSkillTileState SelectSkillTileState => _selectSkillTileState;

        public PlayerUseSkillState UseSkillState => _useSkillState;
        

      

        public SkillManager SkillManager => _skillManager;

        

        #endregion

        [Inject]
        public void Construct(InputManager inputManager, 
            ICommandPublisher publisher, ICommandSubscribable subscribable,
            SkillManager skillManager)
        {
            _inputManager = inputManager;
          
            _publisher = publisher;
            _subscribable = subscribable;
            _skillManager = skillManager;
        }

        protected override void Awake()
        {
            base.Awake();
            _playerSelectMoveTileState = new PlayerSelectMoveTileState(this, string.Empty);
            _moveState = new PlayerMoveState(this, string.Empty);
            _idleState = new PlayerIdleState(this, string.Empty);
           
            _selectSkillTileState = new PlayerSelectSkillTileState(this, string.Empty);
            _useSkillState = new PlayerUseSkillState(this, string.Empty);
            _hurtState = new HurtState(this, string.Empty);
            _deadState = new DeadState(this, string.Empty);
            AddState(_idleState);
            AddState(_playerSelectMoveTileState);
            AddState(_selectSkillTileState);
            AddState(_moveState);
            AddState(_useSkillState);
            AddState(_hurtState);
            AddState(_deadState);
            ChangeState(_idleState);
        }

        public override void Init(EntityMetadata metadata, int teamID)
        {
            base.Init(metadata, teamID);
            RemainStamina = statsSystem.Stats.Stamina;
            RemainMana = statsSystem.Stats.Mana;
        }


        public override void OnEnter()
        {
            base.OnEnter();
          
            _publisher.PublishAsync(new EnableEndTurnCommand() { IsEnable = true });
            _publisher.PublishAsync(new EnableSkillCommand() { IsEnable = true });
            
            _inputManager.OnEnter();
           
            ChangeState(_playerSelectMoveTileState);
        }

        public override void OnUpdate()
        {
            _stateMachine.Update();
        }

        public override void OnExit()
        {
            RemainStamina = statsSystem.Stats.Stamina;
            RemainMana = statsSystem.Stats.Mana;
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


        public override bool IsDead()
        {
            return false;
        }
    }
}