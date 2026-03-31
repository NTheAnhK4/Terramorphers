using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace GameCore.Domain.Skill{
    [Serializable]
    public class SkillMetadata
    {
        [SerializeField] private int skillID;
        [SerializeField][PreviewField(height:50)] private Sprite skillSprite;
        [SerializeField] private ESkillRarity _eSkillRarity;
        [SerializeField] private string skillName;
        [SerializeField] private int costs;
        
        [SerializeField] private int range;

        [SerializeField] private List<ESkillTargetType> skillTargetTypes;
        [SerializeReference] private List<ISkillHandler> skillHandlers = new();

        public Sprite SkillSprite => skillSprite;

        public ESkillRarity ESkillRarity => _eSkillRarity;

        public string SkillName => skillName;

        public int SkillCosts => costs;

        public int SkillID => skillID;

        public IReadOnlyList<ESkillTargetType> SkillTargetTypes => skillTargetTypes;

        public int Range => range;

        public void Apply(MonoBehaviour context)
        {
            foreach (var skillHandler in skillHandlers) skillHandler.Apply(context);
        }
    }

}
