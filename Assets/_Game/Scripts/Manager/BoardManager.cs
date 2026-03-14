using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame;
using System.IO;
using Cysharp.Threading.Tasks;
using Terramorphers.Command;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VitalRouter;
using R3;

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
        [Inject] private ICommandSubscribable _subscribable;
        private List<List<ITile>> board = new();
        AsyncOperationHandle<TextAsset> handle;

        public List<List<ITile>> Board => board;
        public string path = "Assets/_Game/Scripts/Configs/GameConfig.json";
        private List<ITile> currentSpecialTiles = new();
        private List<IDisposable> bags = new();
        #region JSonHandler

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
                    tile.Transform.position = positionData.rows[i].positions[j];
                   
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

        #endregion

        private void Start()
        {
            bags.Add(_subscribable.Subscribe<SetMovableTilesCommand>(SetMovableTiles)); 
            bags.Add( _subscribable.Subscribe<ClearSpecialTilesCommand>(ClearSpecialTiles));
        }

        private void OnDestroy()
        {
            foreach(var bag in bags) bag?.Dispose();
        }

      

        public List<ITile> GetPassableTile()
        {
            if (board == null) return new List<ITile>();
            var allTiles = board.SelectMany(row => row).ToList();
            
            return allTiles.Where(t => t.IsPassable()).ToList();
        }

        public List<ITile> GetPath(ITile from, ITile to) => board.GetPath(from, to, tile => tile.IsPassable());

      
        private void SetMovableTiles(SetMovableTilesCommand tilesCommand, PublishContext context)
        {
            ClearSpecialTiles();
           
            List<ITile> movableTiles = board.GetTileMovable(tilesCommand.CenterTile, tilesCommand.Distance, tile => tile.GetMoveCost());
            currentSpecialTiles = movableTiles;
            foreach (var tile in movableTiles)
            {
                tile.ChangeState(ETileState.Movable);
            }
        }

       
        private void ClearSpecialTiles(ClearSpecialTilesCommand command, PublishContext context)
        {
            ClearSpecialTiles();
        }

        public void ClearSpecialTiles()
        {
            if (currentSpecialTiles == null) return;
            foreach (var tile in currentSpecialTiles)
            {
                tile.ChangeState(ETileState.Normal);
            }
            currentSpecialTiles.Clear();
        }

        
    }
}