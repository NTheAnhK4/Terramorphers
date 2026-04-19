using System;
using GameCore.Domain.Skill;


namespace GameCore.Domain.Stats
{
    public class EntityStatModifier : StatModifier
    {
        private readonly EStatsType type;
        private readonly Func<int, int> operation;
       

        public override void Handle(object sender, Query query)
        {
            if (query.StatsType == type)  query.Value = operation(query.Value);
        }


        public EntityStatModifier( int turnApplyValue, EStatsType type, Func<int,int> operation, IEffect effect) : base( turnApplyValue, effect)
        {
            this.type = type;
            this.operation = operation;
        }
    }
}