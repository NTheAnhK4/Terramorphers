
using System;
using System.Collections.Generic;
using CoreGame;
using Sirenix.Serialization;
using Terramorphers.States.EnemyState;
using UnityEngine;

namespace Terramorphers
{
    public class SatyrRider : Enemy
    {
        #region Tmp Value

        public int AttackRange = 1;
        #endregion
        #region Animation Hash

        private readonly int idleAnimHash = Animator.StringToHash("Idle");
        private readonly int attackAnimHash = Animator.StringToHash("Attacking");
        private readonly int dyingAnimHash = Animator.StringToHash("Dying");
        private readonly int idleBlinkAnimHah = Animator.StringToHash("IdleBlink");
        private readonly int hurtAnimHash = Animator.StringToHash("Hurt");
        private readonly int walkAnimHash = Animator.StringToHash("Walking");
        private readonly int tauntAnimHash = Animator.StringToHash("Taunt");
        #endregion

        #region State

        private SatyrRiderThinkingState _thinkingState;
        private EnemyIdleState _idleState;
        private EnemyMoveState _moveState;
     

        public EnemyIdleState IdleState => _idleState;

        public EnemyMoveState MoveState => _moveState;

        #endregion

        public override void TurnToThinkingState()
        {
            base.TurnToThinkingState();
            ChangeState(_thinkingState);
        }

        protected override void Awake()
        {
            base.Awake();
            _idleState = new EnemyIdleState(this,
                new List<int>() { idleAnimHash, idleBlinkAnimHah },
                new List<List<float>>()
                {
                    new List<float>(){.7f, .3f},
                    new List<float>(){.95f,.05f}
                    
                });
            _thinkingState = new SatyrRiderThinkingState(this, idleAnimHash);
            _moveState = new EnemyMoveState(this, walkAnimHash);
            AddState(_idleState);
            AddState(_thinkingState);
            AddState(_moveState);
            ChangeState(_idleState);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            SetStateWithMinCost(_thinkingState);
            CaculateMinCost();
        }
        
        public override void OnExit()
        {
            base.OnExit();
            SetStateWithMinCost(_idleState);
            CaculateMinCost();
          
        }
    }

}
