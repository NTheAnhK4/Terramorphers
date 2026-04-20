
using System;
namespace GameCore.Domain.Stats
{
    public class StatsSystem
    {
        
        public Stats Stats;

        public StatsSystem(EntityStats stats)
        {
            Stats = new Stats(new StatsMediator(), stats);
        }
    
        public void AddModifier(StatModifier modifier)
        {
            Action onRemoved = null;
            Stats.Mediator.AddModifier(modifier, onRemoved);
        }

        public void RemoveModifier(StatModifier modifier)
        {
            Stats.Mediator.RemoveModifier(modifier);
        }

        public void HandeEvent<T> (T  owner, EEffectTriggerType trigger) => Stats?.Mediator.HandleEvent(owner, trigger);

        public void Update() => Stats?.Mediator.Update();

    }
}