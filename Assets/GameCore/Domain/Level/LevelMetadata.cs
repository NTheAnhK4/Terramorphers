using System;
using System.Collections.Generic;
using GameCore.Domain.Quest;
using GameCore.Domain.Reward;
using GameCore.Domain.Tile;

using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.Domain.Level
{
    [Serializable]
    public class LevelMetadata
    {
        [SerializeField] private string levelName;
        [HorizontalGroup("Sprites")]
        [PreviewField(Height = 50)]
        [SerializeField]
        private Sprite levelSprite;

        [HorizontalGroup("Sprites")]
        [PreviewField(Height = 50)]
        [SerializeField]
        private Sprite backgroundSprite;

        [HorizontalGroup("Reward")] [SerializeField]
        private List<StageRewardData> rewardData = new();
        [SerializeField, TableList] private List<LevelStageData> levelStageDatas = new();

        public Sprite LevelSprite => levelSprite;

        public string LevelName => levelName;

        public Sprite BackgroundSprite => backgroundSprite;

        public IReadOnlyList<LevelStageData> LevelStageDatas => levelStageDatas;

        public IReadOnlyList<StageRewardData> RewardData => rewardData;

        private void OnValidate()
        {
            
        }
        
    }

    [Serializable]
    public class LevelStageData
    {
        [VerticalGroup("levelStage")]
        [HorizontalGroup("levelStage/levelInfo")]
        [VerticalGroup("levelStage/levelInfo/left")]
        [HideLabel]
        [SerializeField] private TextAsset stageMap;

       
        [VerticalGroup("levelStage")]
        [HorizontalGroup("levelStage/levelInfo")]
        [HideLabel]
        [SerializeField] private List<int> enemyIDs = new();

        [VerticalGroup("levelStage")]
        [HorizontalGroup("levelStage/objective")] [SerializeField, HideLabel]
        private List<QuestMetadata> stageStarObjectives = new();

        public IReadOnlyList<int> EnemyIDs => enemyIDs;

        public TextAsset StageMap => stageMap;

     

        public IReadOnlyList<QuestMetadata> StageStarObjectives => stageStarObjectives;
    }

    [Serializable]
    public class SpawnSlotData
    {
        public int TeamID;
        public Vector2Int Position;
    }
    [Serializable]
    public class MapData
    {
        public List<SpawnSlotData> SlotDatas;
        public List<MapRow> Rows;
    }

    [Serializable]
    public class MapRow
    {
        public List<ETileType> Tiles;
    }

    [Serializable]
    public class StageRewardData
    {
        [HorizontalGroup("col")]
        [VerticalGroup("col/row")]
        [SerializeField] private int minStageRewquired;

        [VerticalGroup("col/row")] [SerializeField]
        private int requiredStars;
        
        [HorizontalGroup("col")][SerializeField] private RewardItemData rewardItemData;

        public int MinStageRewquired => minStageRewquired;

        public RewardItemData RewardItemData => rewardItemData;

        public int RequiredStars => requiredStars;
        
    }

    public class StageRewardItem
    {
        public ERewardItemType RewardItemType;
        public int SkillID;
        public int Amount;
    }

}