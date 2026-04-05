namespace Terramorphers.Stats
{
    [System.Serializable]
    public class EntityStats
    {
        public int MaxHP;
        public int Mana;
        public int Stamina;
        public int Range;
        public int PhysicalDamage;
        public int MagicalDamage;
        public int PhysicalResistance;
        public int MagicalResistance;
        public int NeutralResistance;
        public int CriticalChance;
        public int Healing;
        
    }

    public class Stats
    {
        private readonly StatsMediator mediator; // Mediator responsible for modifying stat queries dynamically
        public readonly EntityStats EntityStats; // The base stats data

        // Property to expose the mediator (read-only)
        public StatsMediator Mediator => mediator;
        public Stats(StatsMediator mediator, EntityStats entityStats)
        {
            this.mediator = mediator;
            this.EntityStats = entityStats;
        }

        public int Mana
        {
            get
            {
                var q = new Query(EStatsType.Mana, EntityStats.Mana);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
        public int Stamina
        {
            get
            {
                var q = new Query(EStatsType.Stamina, EntityStats.Stamina);mediator.PerformQuery(this,q);
                return q.Value;
            }
        }
        public int Range
        {
            get
            {
                var q = new Query(EStatsType.Range, EntityStats.Range);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }

        public int MaxHP
        {
            get
            {
                var q = new Query(EStatsType.MaxHP, EntityStats.MaxHP);
                mediator.PerformQuery(this,q);
                return q.Value;
            }
        }

        public int PhysicalDamage
        {
            get
            {
                var q = new Query(EStatsType.PhysicalDamage, EntityStats.PhysicalDamage);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }

        public int MagicalDamage
        {
            get
            {
                var q = new Query(EStatsType.MagicalDamage, EntityStats.MagicalDamage);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }

        public int PhysicalResistance
        {
            get
            {
                var q = new Query(EStatsType.PhysicalResistance, EntityStats.PhysicalResistance);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }

        public int MagicalResistance
        {
            get
            {
                var q = new Query(EStatsType.MagicalResistance, EntityStats.MagicalResistance);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }

        public int NeutralResistance
        {
            get
            {
                var q = new Query(EStatsType.NeutralResistance, EntityStats.NeutralResistance);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }

        public int CriticalChance
        {
            get
            {
                var q = new Query(EStatsType.CriticalChance, EntityStats.CriticalChance);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }

        public int Healing
        {
            get
            {
                var q = new Query(EStatsType.Healing, EntityStats.Healing);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }
    }
}

