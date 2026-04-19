using System;
using GameCore.Domain.Skill;
using GameCore.Utility;

namespace GameCore.Domain.Stats
{
    public abstract class StatModifier : IDisposable
    {
        public bool MarkedForRemoval{get; private set; }
        public event Action<StatModifier> OnDispose = delegate { };
        private readonly CountdownTimer timer;
        public abstract void Handle(object sender, Query query);
        private IEffect _effect;

        public Action OnRemoved;
        protected StatModifier(int turnApplyValue, IEffect effect)
        {
           
         
            
            if (turnApplyValue <= 0) return;
            _effect = effect;
            timer = new CountdownTimer(turnApplyValue);

            timer.OnTimerStop += () => MarkedForRemoval = true;
            timer.Start();
        }

        public void HandleEvent<T>(T owner, EEffectTriggerType trigger)
        {
            if (_effect != null) _effect.Execute(owner, trigger);
        }
        public void Update()
        {
            
            timer.Tick(1);
        }

        public void Dispose()
        {
            OnRemoved?.Invoke();
            OnDispose.Invoke(this);
        }
    }
}