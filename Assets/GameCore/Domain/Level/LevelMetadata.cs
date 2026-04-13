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
        [SerializeField] private TextAsset stageMap;
        [SerializeField] private List<int> enemyIDs = new();

        public IReadOnlyList<int> EnemyIDs => enemyIDs;

        public TextAsset StageMap => stageMap;
    }
    [Serializable]
    public class BoardData
    {
        public List<BoardRow> Rows;
    }

    [Serializable]
    public class BoardRow
    {
        public List<ETileType> Tiles;
    }

}