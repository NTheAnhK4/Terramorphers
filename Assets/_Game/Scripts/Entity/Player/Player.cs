
using GameCore.Commands;
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
        private BoardManager _boardManager;
        private ICommandPublisher _publisher;
        private ICommandSubscribable _subscribable;

        #endregion

        #region Runtime Data

        private ITile selectedTile;
        private int stamina = 3;
       
        private float moveSpeed = .5f;
        private int remainStamina;

        #endregion

        #region State

        private SelectMoveTileState _selectMoveTileState;
        private PlayerMoveState _moveState;
        private EntityWaitingState _waitingState;
        private PlayerSelectSkillTileState _selectSkillTileState;

        #endregion
        #region Properties

      

        public int RemainStamina
        {
            get => remainStamina;
            set
            {
                if (value != remainStamina)
                {
                    if (_publisher != null) _publisher.PublishAsync(new SetRemainStaminaCommand() { RemainStamina = value });
                    remainStamina = value;
                }
                
            }
        }

        public ICommandPublisher Publisher => _publisher;

        public ICommandSubscribable Subscribable => _subscribable;

        public InputManager InputManager => _inputManager;

        public SelectMoveTileState SelectMoveTileState => _selectMoveTileState;

        public PlayerMoveState MoveState => _moveState;

        public PlayerSelectSkillTileState SelectSkillTileState => _selectSkillTileState;

        public BoardManager BoardManager => _boardManager;

        public float MoveSpeed => moveSpeed;
        public ITile SelectedTile
        {
            get => selectedTile;
            set => selectedTile = value;
        }

        #endregion
        [Inject]
        public void Construct(InputManager inputManager, BoardManager boardManager, ICommandPublisher publisher, ICommandSubscribable subscribable)
        {
            _inputManager = inputManager;
            _boardManager = boardManager;
            _publisher = publisher;
            _subscribable = subscribable;
        }

        protected override void Awake()
        {
            base.Awake();
            _selectMoveTileState = new SelectMoveTileState(this, string.Empty);
            _moveState = new PlayerMoveState(this, string.Empty);
            _waitingState = new EntityWaitingState(this, string.Empty);
            _selectSkillTileState = new PlayerSelectSkillTileState(this, string.Empty);
            AddState(_waitingState);
            AddState(_selectMoveTileState);
            AddState(_selectSkillTileState);
            AddState(_moveState);
        }

        public override void OnEnter()
        {
            _publisher.PublishAsync(new ToggleEndTurnCommand() { IsOn = true });
            remainStamina = stamina;
            _inputManager.OnEnter();
            _publisher.PublishAsync(new SetRemainStaminaCommand() { RemainStamina = remainStamina });
            ChangeState(_selectMoveTileState);
        }

        public override void OnUpdate()
        {
           _stateMachine.Update();
        }

        public override void OnExit()
        {
            ChangeState(_waitingState);
            _publisher.PublishAsync(new ClearSpecialTilesCommand());
            _publisher.PublishAsync(new ToggleEndTurnCommand() { IsOn = false });
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
