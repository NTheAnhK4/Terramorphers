using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame;
using System.IO;

using UnityEngine;
using VContainer;


namespace Terramorphers
{
    [Serializable]
    public class TilePositionData
    {
        public List<RowData> rows = new();
    }

    [Serializable]
    public class RowData
    {
        public List<Vector3> positions = new();
    }
    public class BoardManager : Singleton<BoardManager>
    {
        
        private List<List<ITile>> board = new();

        public List<List<ITile>> Board => board;
        public string path = "Assets/_Game/Scripts/Configs/GameConfig.json";
        
        
        
        public void SaveToJson()
        {
            TilePositionData data = new();

            foreach (var row in board)
            {
                RowData rowData = new();

                var sortedPositions = row
                    .OfType<BasicTile>()                
                    .Select(t => t.transform.position)  
                    .OrderBy(p => p.x)                  
                    .ToList();

                rowData.positions.AddRange(sortedPositions);
                data.rows.Add(rowData);
            }

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);

            Debug.Log("Saved to: " + path);
        }
        public TilePositionData LoadFromJson()
        {

            if (!File.Exists(path))
            {
                Debug.LogWarning("File not found!");
                return null;
            }

            string json = File.ReadAllText(path);

            TilePositionData data = JsonUtility.FromJson<TilePositionData>(json);
            
            Debug.Log("Loaded JSON");
            return data;
        }

        private void Start()
        {
        }

        public void SetMovable(int posX, int posY, int distance)
        {
            
        }
    }
}