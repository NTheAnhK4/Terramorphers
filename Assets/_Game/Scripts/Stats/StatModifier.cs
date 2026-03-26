using System;
using GameCore.Utility;

namespace Terramorphers.Stats
{
    public abstract class StatModifier : IDisposable
    {
        public bool MarkedForRemoval{get; private set; }
        public event Action<StatModifier> OnDispose = delegate { };
        private readonly CountdownTimer timer;
        public abstract void Handle(object sender, Query query);

        public Action OnRemoved;
        protected StatModifier(int turnApplyValue)
        {
           
         
            
            if (turnApplyValue <= 0) return;
            timer = new CountdownTimer(turnApplyValue);

            timer.OnTimerStop += () => MarkedForRemoval = true;
            timer.Start();
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