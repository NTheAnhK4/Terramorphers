using System;
using System.Collections.Generic;
using GameCore.Domain.Quest;
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
        [SerializeField, TableList] private List<LevelStageData> levelStageDatas = new();

        public Sprite LevelSprite => levelSprite;

        public string LevelName => levelName;

        public Sprite BackgroundSprite => backgroundSprite;

        public IReadOnlyList<LevelStageData> LevelStageDatas => levelStageDatas;
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

}