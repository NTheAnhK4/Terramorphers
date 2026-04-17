using System;
using System.Collections.Generic;
using GameCore.Domain.Tile;
using Sirenix.OdinInspector;
using UnityEngine;

using UtilityAI.Considerations;

namespace Terramorphers
{
    [CreateAssetMenu(menuName = "Database/EntityData/EnemyMetadata", fileName = "EnemyMetadata")]
    public class EnemyMetadata : EntityMetadata
    {
        [TabGroup("Consideration",Icon =SdfIconType.Cpu, TextColor = "cyan")] [SerializeField] private ConsiderationSystem _considerationSystem;
        [TabGroup("Skill", Icon = SdfIconType.Lightning, TextColor = "red")] [SerializeField, TableList] private List<SkillConsiderationData> skillConsiderationDatas = new();

        [TabGroup("Consideration")] [SerializeField] private int selfConsiderationID  = -1;
        [TabGroup("Consideration")] [SerializeField] private int allyConsiderationID = -1;
        [TabGroup("Consideration")] [SerializeField] private int enemyConsiderationID = -1;
        [TabGroup("Consideration")] [SerializeField] private int useSkillActionConsiderationID;

        [TabGroup("Consideration")] [SerializeField] private int moveActionConsiderationID;

        [TabGroup("Consideration")] [SerializeField]
        private int endTurnActionConsiderationID;
     
        [TabGroup("Tile", Icon = SdfIconType.HeptagonFill, TextColor = "green")] [SerializeField, TableList] private List<TileConsiderationData> tileConsiderationDatas = new();
        [TabGroup("Consideration")] [SerializeField] private int commonTileConsiderationID;

        public ConsiderationSystem ConsiderationSystem => _considerationSystem;
        
        public IReadOnlyList<SkillConsiderationData> SkillConsiderationDatas => skillConsiderationDatas;

        public IReadOnlyList<TileConsiderationData> TileConsiderationDatas => tileConsiderationDatas;

        public int CommonTileConsiderationID => commonTileConsiderationID;

        public int UseSkillActionConsiderationID => useSkillActionConsiderationID;

        public int MoveActionConsiderationID => moveActionConsiderationID;

        public int SelfConsiderationID => selfConsiderationID;

        public int EnemyConsiderationID => enemyConsiderationID;

        public int AllyConsiderationID => allyConsiderationID;

        public int EndTurnActionConsiderationID => endTurnActionConsiderationID;

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
                    TileType = tileType,
                    ConsiderationID = -1,
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
        [TableColumnWidth(50)]
        [SerializeField] private int skillID;
        [TableColumnWidth(50)]
        [SerializeField] private int considerationID;
        [TableColumnWidth(300)]
        [SerializeField] private string animName;
       

        public int SkillID => skillID;

        public string AnimName => animName;

        public int ConsiderationID => considerationID;
    }

    [Serializable]
    public class TileConsiderationData
    {
        [HorizontalGroup("tileConsiderationData", Width = .4f)] [HideLabel]
        public ETileType TileType;
        [HorizontalGroup("tileConsiderationData", Width = .6f)] [HideLabel]
        public int ConsiderationID;
    }

}