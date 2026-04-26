using System;
using Sirenix.OdinInspector;
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
        [TabGroup("General")]  public string curentState;
        [TabGroup("General")] public bool IsAnimationTriggerFinished;
        [TabGroup("Components")] public Animator Anim;

        [SerializeField, TabGroup("Components")]
        protected SpriteRenderer entitySpriteRenderer;
        protected StateMachine _stateMachine;
        [TabGroup("Components")] public Transform Model;
        public IState CurrentState => _stateMachine?.State;
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (Anim == null) Anim = GetComponentInChildren<Animator>();
            if (Model == null) Model = transform.Find("Model");
            if (entitySpriteRenderer == null) entitySpriteRenderer = transform.Find("Model").GetComponent<SpriteRenderer>();
        }

        protected override void Awake()
        {
            _stateMachine = new StateMachine();
            IsAnimationTriggerFinished = true;
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