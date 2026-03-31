using CoreGame;
using UnityEngine;
using System;
namespace Terramorphers.Stats
{
    public class StatsSystem 
    {
        [SerializeField] private Entity entity;
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

        public void Update() => Stats?.Mediator.Update();

    }
}