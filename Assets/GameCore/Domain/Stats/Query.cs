namespace GameCore.Domain.Stats
{
    public class Query
    {
        public readonly EStatsType StatsType;
        public int Value;

        public Query(EStatsType statsType, int value)
        {
            this.StatsType = statsType;
            this.Value = value;
        }
    }
}