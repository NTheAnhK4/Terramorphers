using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Terramorphers.Stats;
using UnityEngine;
using UtilityAI.AIActions;


namespace Terramorphers
{
    [Serializable]
    public class EntityMetadata
    {
        [Serializable]
        public class SkillAnim
        {
            public int SkillID;
            public string AnimName;
        }
        [SerializeField] private string addressable;
        [SerializeField] private EntityStats _entityStats;
        [SerializeField]
        private List<AIAction> _actions = new();

        [SerializeField, TableList] private List<SkillAnim> skillAnims = new();
    
        public string Addressable => addressable;

        public EntityStats EntityStats => _entityStats;

        public IReadOnlyList<AIAction> Actions => _actions;

        public IReadOnlyList<SkillAnim> SkillAnims => skillAnims;
    }

}
