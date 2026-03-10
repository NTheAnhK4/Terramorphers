using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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

    public class BoardManager : ComponentBehaviour
    {
        [Inject] private ITileFactory _tileFactory;
        private List<List<ITile>> board = new();
        AsyncOperationHandle<TextAsset> handle;

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

        public async UniTask<bool> LoadingBoard(LevelMetadata levelMetadata)
        {
            TilePositionData positionData = LoadFromJson();
            if (positionData == null) return false;
            handle = Addressables.LoadAssetAsync<TextAsset>(levelMetadata.BoardDataAddressable);

            TextAsset jsonFile = await handle.Task;
            if (jsonFile == null)
            {
                Debug.Log($"[BoardManager] load file from json is failure");
                return false;
            }
            BoardData boardData = JsonUtility.FromJson<BoardData>(jsonFile.text);

           
          
            board.Clear();
            _tileFactory.SetParent(transform);
            for (int i = 0; i < boardData.Rows.Count; ++i)
            {
                List<ITile> row = new();
               
                for (int j = 0; j < boardData.Rows[i].Tiles.Count(); ++j)
                {
                    ETileType type = boardData.Rows[i].Tiles[j];
                    var tile = await _tileFactory.CreateTile(type);
                    if (tile == null) return false;
                    if (tile is BasicTile basicTile) basicTile.transform.position = positionData.rows[i].positions[j];
                   
                    row.Add(tile);
                }

                board.Add(row);
            }

            return true;
        }

        public void ReleaseBoardData()
        {
            if (handle.IsValid()) Addressables.Release(handle);
        }

        private List<ITile> GetPassableTile()
        {
            if (board == null) return new List<ITile>();
            var allTiles = board.SelectMany(row => row).ToList();
            
            return allTiles.Where(t => t.IsPassable()).ToList();
        }
        


        public void SetMovable(int posX, int posY, int distance)
        {
        }
    }
}