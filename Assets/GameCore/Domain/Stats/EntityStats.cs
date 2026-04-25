using System;
using GameCore.Domain.Skill;
using UnityEngine;

namespace GameCore.Domain.Stats
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

        public int GetRawStatValue(EStatsType stats)
        {
            switch (stats)
            {
                case EStatsType.None: return 0;
                case EStatsType.MaxHP: return EntityStats.MaxHP;
                case EStatsType.Mana: return EntityStats.Mana;
                case EStatsType.Stamina: return EntityStats.Stamina;
                case EStatsType.Range: return EntityStats.Range;
                case EStatsType.PhysicalDamage: return EntityStats.PhysicalDamage;
                case EStatsType.MagicalDamage: return EntityStats.MagicalDamage;
                case EStatsType.PhysicalResistance: return EntityStats.PhysicalResistance;
                case EStatsType.MagicalResistance: return EntityStats.MagicalResistance;
                case EStatsType.NeutralResistance: return EntityStats.NeutralResistance;
                case EStatsType.CriticalChance: return EntityStats.CriticalChance;
                case EStatsType.Healing: return EntityStats.Healing;
                default: return 0;
            }
        }

        public int GetStatValue(EStatsType statsType)
        {
            var q = new Query(statsType, GetRawStatValue(statsType));
            mediator.PerformQuery(this, q);
            return q.Value;
        }
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

        public int GetDamage(int rawDamage, EAttackType attackType)
        {
            float addedDamage = 0;
            switch (attackType)
            {
                case EAttackType.PhysicalDamage:
                    addedDamage += 1.0f * PhysicalDamage / 100 * rawDamage;
                    break;
                case EAttackType.MagicalDamage:
                    addedDamage += 1.0f * MagicalDamage / 100 * rawDamage;
                    break;
                case EAttackType.NeutralDamage:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null);
            }
    
            return Mathf.Max(rawDamage, rawDamage + Mathf.RoundToInt(addedDamage));
        }

        public int GetDamageTaken(int rawDamage, EAttackType attackType)
        {
            float reduceDamage = 0;
            switch (attackType)
            {
                case EAttackType.PhysicalDamage:
                    reduceDamage += 1.0f * PhysicalResistance / 100 * rawDamage;
                    break;
                case EAttackType.MagicalDamage:
                    reduceDamage += 1.0f * MagicalDamage / 100 * rawDamage;
                    break;
                case EAttackType.NeutralDamage:
                    reduceDamage += 1.0f * NeutralResistance / 100 * rawDamage;
                    break;
                
            }

            return Mathf.Clamp(rawDamage - Mathf.RoundToInt(reduceDamage), 0, rawDamage);
        }

        public int GetHealAmount(int rawHealAmount) => rawHealAmount + Healing;
    }
}

