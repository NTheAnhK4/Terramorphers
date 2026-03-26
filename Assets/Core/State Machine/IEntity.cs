using System;
using UnityEngine;

namespace CoreGame
{
    
    public interface IEntity
    {
        void ChangeState(IState newState, Func<StateData> getData = null);
        void AnimationFinishTrigger();
        void AnimationTrigger();
    }
    [Serializable]
    public class Entity : ComponentBehaviour, IEntity
    {
        public string curentState;
        public bool IsAnimationTriggerFinished;
        public Animator Anim;
        protected StateMachine _stateMachine;
        public Transform Model;
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (Anim == null) Anim = GetComponentInChildren<Animator>();
            if (Model == null) Model = transform.Find("Modal");
        }

        protected override void Awake()
        {
            _stateMachine = new StateMachine();
        }

        public void To(IState from, IState to, IPredicate condition, Func<StateData> getData = null) => _stateMachine.AddTransition(from, to, condition, getData);
        public void Any(IState to, IPredicate condition, Func<StateData> getData = null) => _stateMachine.AddAnyTransition(to, condition, getData);
        public void ChangeState(IState newState, Func<StateData> getData = null) =>  _stateMachine.ChangeState(newState, getData);

        protected virtual void AddState(IState newState) => _stateMachine.AddState(newState);

        public virtual void AnimationFinishTrigger()
        {
            if(_stateMachine.State != null) _stateMachine.State.AnimationFinishTrigger();
        }

        public virtual void AnimationTrigger()
        {
            if(_stateMachine.State != null) _stateMachine.State.AnimationTrigger();
        }
    }
}