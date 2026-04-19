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
        [SerializeField] private ESkillEffectType skillEffectType;
        [SerializeField] private EStatsType statsType;
        [SerializeField] private ESkillOperationType operationType;
        [SerializeField] private int value;
        [SerializeField] private int turnApply;

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
            return new EntityStatModifier(turnApply, statsType, func);
        }
        
    }
}