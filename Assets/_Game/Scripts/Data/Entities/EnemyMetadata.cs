using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UtilityAI.AIActions;
using UtilityAI.Considerations;

namespace Terramorphers
{
    [CreateAssetMenu(menuName = "Database/EntityData/EnemyMetadata", fileName = "EnemyMetadata")]
    public class EnemyMetadata : EntityMetadata
    {
        
        [SerializeField] private List<SkillConsiderationData> skillConsiderationDatas = new();
        [SerializeField] private List<AIAction> aiActions = new();
        [SerializeField, TableList] private List<TileConsiderationData> tileConsiderationDatas = new();
        [SerializeField, TableList] private Consideration commonTileConsideration;

        public IReadOnlyList<AIAction> AIActions => aiActions;
        public IReadOnlyList<SkillConsiderationData> SkillConsiderationDatas => skillConsiderationDatas;

        public IReadOnlyList<TileConsiderationData> TileConsiderationDatas => tileConsiderationDatas;
        
        public Consideration CommonTileConsideration => commonTileConsideration;
#if UNITY_EDITOR
        [Button]
        private void Bake()
        {
            Dictionary<ETileType, TileConsiderationData> tileTypeToConsideration = new();
            foreach (var tileConsideration in tileConsiderationDatas)
            {
                tileTypeToConsideration[tileConsideration.TileType] = tileConsideration;
            }

            foreach (ETileType tileType in Enum.GetValues(typeof(ETileType)))
            {
                if (tileTypeToConsideration.ContainsKey(tileType)) continue;
                TileConsiderationData tileConsiderationData = new TileConsiderationData()
                {
                    TileType = tileType
                };
                tileConsiderationDatas.Add(tileConsiderationData);
                tileTypeToConsideration[tileType] = tileConsiderationData;
            }
        }
    
        #endif
    }
    [Serializable]
    public class SkillConsiderationData
    {
        [SerializeField] private int skillID;
        [SerializeField] private string animName;
        [SerializeField] private List<Consideration> consideration = new();

        public int SkillID => skillID;

        public string AnimName => animName;

        public IReadOnlyList<Consideration> Consideration => consideration;
    }

    [Serializable]
    public class TileConsiderationData
    {
        public ETileType TileType;
        [SerializeField] private Consideration consideration;

        public Consideration Consideration => consideration;
    }

}