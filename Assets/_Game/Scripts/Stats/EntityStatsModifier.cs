using System;
namespace Terramorphers.Stats
{
    public class EntityStatsModifier : StatModifier
    {
        private readonly EStatsType type;
        private readonly Func<int, int> operation;
       

        public override void Handle(object sender, Query query)
        {
            if (query.StatsType == type) query.Value = operation(query.Value);
        }


        public EntityStatsModifier( int turnApplyValue, EStatsType type, Func<int,int> operation) : base( turnApplyValue)
        {
            this.type = type;
            this.operation = operation;
        }
    }
}