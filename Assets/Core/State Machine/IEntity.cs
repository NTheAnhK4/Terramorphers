using System;
using UnityEngine;

namespace CoreGame
{
    public interface IEntity
    {
    }

    public class Entity : ComponentBehaviour, IEntity
    {
        public string curentState;
        public bool IsAnimationTriggerFinished;
        public Animator Anim;
        protected StateMachine _stateMachine;

        protected override void Awake()
        {
            _stateMachine = new StateMachine();
        }

        public void AddTransition(IState from, IState to, IPredicate condition, Func<StateData> getData = null) => _stateMachine.AddTransition(from, to, condition, getData);
        public void AddAnyTransition(IState to, IPredicate condition, Func<StateData> getData = null) => _stateMachine.AddAnyTransition(to, condition, getData);
        public void ChangeState(IState newState, Func<StateData> getData = null) => _stateMachine.ChangeState(newState, getData);
        public void AddState(IState newState) => _stateMachine.AddState(newState);
    }
}