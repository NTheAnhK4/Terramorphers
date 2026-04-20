using GameCore.Domain.Stats;

namespace Terramorphers
{
    public class AmbushTile : BaseTile
    {
        private StatModifier _statModifier;
        public override bool IsPassable() => true;

        public override bool IsBlockVisibility() => false;
        public override int GetMoveCost() => 1;

        private StatModifier GetEffect()
        {
            return _statModifier ?? new EntityStatModifier(-1, EStatsType.PhysicalDamage, t => t + 25, null);
        }

        public override void ApplyEffect(TerramorphersEntity entity)
        {
            base.ApplyEffect(entity);
            entity.StatsSystem.AddModifier(GetEffect());
        }

        public override void RemoveEffect(TerramorphersEntity entity)
        {
            base.RemoveEffect(entity);
            entity.StatsSystem.RemoveModifier(GetEffect());
        }
    }
}