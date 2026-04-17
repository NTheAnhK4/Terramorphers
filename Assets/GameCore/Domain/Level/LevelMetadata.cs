using System;
using System.Collections.Generic;
using GameCore.Domain.Tile;

using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore.Domain.Level
{
    [Serializable]
    public class LevelMetadata
    {
        [SerializeField] private string levelName;
        [SerializeField, PreviewField(Height = 50)]
        private Sprite levelSprite;

        [SerializeField, TableList] private List<LevelStageData> levelStageDatas = new();

        public Sprite LevelSprite => levelSprite;

        public string LevelName => levelName;

        public IReadOnlyList<LevelStageData> LevelStageDatas => levelStageDatas;
    }

    [Serializable]
    public class LevelStageData
    {
        [HorizontalGroup("levelStageRow")]
        [VerticalGroup("levelStageRow/left")]
        [HideLabel]
        [SerializeField] private TextAsset stageMap;

        [VerticalGroup("levelStageRow/left")] [SerializeField, PreviewField(Height = 75,Alignment = ObjectFieldAlignment.Center)]
        [HideLabel]
        private Sprite background;
        [HorizontalGroup("levelStageRow")]
        [HideLabel]
        [SerializeField] private List<int> enemyIDs = new();

        public IReadOnlyList<int> EnemyIDs => enemyIDs;

        public TextAsset StageMap => stageMap;

        public Sprite Background => background;
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