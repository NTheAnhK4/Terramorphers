
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private int moveDistance = 3;
        private ITile currentTile;
        private float moveSpeed = .5f;
        private int remainMoveDistance;

        #endregion

        #region State

        private SelectMoveTileState _selectMoveTileState;
        private PlayerMoveState _moveState;
        private EntityWaitingState _waitingState;

        #endregion
        #region Properties

        public ITile CurrentTile => currentTile;

        public int RemainMoveDistance
        {
            get => remainMoveDistance;
            set => remainMoveDistance = value;
        }

        public ICommandPublisher Publisher => _publisher;

        public ICommandSubscribable Subscribable => _subscribable;

        public InputManager InputManager => _inputManager;

        public SelectMoveTileState SelectMoveTileState => _selectMoveTileState;

        public PlayerMoveState MoveState => _moveState;

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
            AddState(_waitingState);
            AddState(_selectMoveTileState);
            
            AddState(_moveState);
        }

        public override void OnEnter()
        {
           
            remainMoveDistance = moveDistance;
            _inputManager.OnEnter();
            ChangeState(_selectMoveTileState);
        }

        public override void OnUpdate()
        {
           _stateMachine.Update();
        }

        public override void OnExit()
        {
            ChangeState(_waitingState);
        }

        public override void SetTile(ITile tile)
        {
            transform.position = tile.Transform.position;
            currentTile = tile;
        }

        

        public override bool IsDead()
        {
            return false;
        }
    }

}
