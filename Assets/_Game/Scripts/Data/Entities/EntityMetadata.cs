using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Terramorphers.Stats;
using UnityEngine;
using UtilityAI.AIActions;


namespace Terramorphers
{
    [CreateAssetMenu(menuName = "Database/EntityData/EntityMetadata", fileName = "EntityMetadata")]
    public class EntityMetadata : ScriptableObject
    {
       
        [SerializeField] private string addressable;
        [SerializeField] private EntityStats _entityStats;
        
    
        public string Addressable => addressable;

        public EntityStats EntityStats => _entityStats;
        
    }
    
    

}
