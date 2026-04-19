using System;
using GameCore.Domain.Stats;
using UnityEngine;

namespace GameCore.Domain.Skill
{
    [Serializable]
    public class SkillEffectData
    {
        public enum ESkillOperationType
        {
            Add,
            Multiple
        };

        public enum EEffectTargetType
        {
            Self,
            Target,
        }

        [SerializeField] private EEffectTargetType targetType;
        [SerializeField] private ESkillEffectType effectType;
        [SerializeField] private EStatsType statsType;
        [SerializeField] private ESkillOperationType operationType;
        [SerializeField] private int value;
        [SerializeField] private int turnApply;
        [SerializeReference] private IEffect effect;

        public StatModifier GetStatModifier()
        {
            Func<int, int> func = null;
            switch (operationType)
            {
                case ESkillOperationType.Add:
                    func = t => t + value;
                    break;
                case ESkillOperationType.Multiple:
                    func = t => Mathf.RoundToInt(1.0f * t * (1 + value) / 100);
                    break;
            }
            return new EntityStatModifier(turnApply, statsType, func, effect);
        }

        public ESkillEffectType EffectType => effectType;

        public EEffectTargetType TargetType => targetType;
    }
}