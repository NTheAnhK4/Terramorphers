using System;
using System.Collections.Generic;
using GameCore.Domain.Entity;
using Sirenix.OdinInspector;
using Terramorphers.Stats;
using UnityEngine;
using UtilityAI.AIActions;


namespace Terramorphers
{
    [CreateAssetMenu(menuName = "Database/EntityData/EntityMetadata", fileName = "EntityMetadata")]
    public class EntityMetadata : BaseEntityMetadata
    {
       
       
        [SerializeField] private EntityStats _entityStats;
        

        public EntityStats EntityStats => _entityStats;
        
    }
    
    

}
