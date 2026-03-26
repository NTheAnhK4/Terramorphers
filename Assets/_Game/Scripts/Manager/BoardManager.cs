using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame;
using System.IO;
using Cysharp.Threading.Tasks;
using GameCore.Utility.Shape;
using Terramorphers.Command;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VitalRouter;


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
        [Inject] private ICommandPublisher _publisher;
        private List<List<ITile>> board = new();
        private HexagonalGrid<ITile> hexaBoard;
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

            hexaBoard = new HexagonalGrid<ITile>(board, ((tile, cube) =>
            {
                tile.Index = cube;
                tile.Transform.name = $"Tile_{cube.q}_{cube.r}_{cube.s}";
            }));

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
            bags.Add(_subscribable.Subscribe<ClearSpecialTilesCommand>(ClearSpecialTiles));
            bags.Add(_subscribable.Subscribe<SetSkillApplicableTilesCommand>(SetSkillApplicableTiles));
        }


        private void OnDestroy()
        {
            foreach (var bag in bags) bag?.Dispose();
        }


        public List<ITile> GetPassableTile()
        {
            if (board == null) return new List<ITile>();
            var allTiles = board.SelectMany(row => row).ToList();

            return allTiles.Where(t => t.IsPassable() && t.CurrentOccupant == null).ToList();
        }

        public List<ITile> GetPath(ITile from, ITile to)
        {
            return hexaBoard.GetPathValue(from.Index, to.Index, tile => tile.GetMoveCost(), tile => !tile.IsPassable() || (tile != from && tile != to && tile.CurrentOccupant != null)).ToList();
        }

        public List<ITile> GetPathWithLimitDistance(ITile from, ITile to, int limitDistance)
        {
            List<ITile> rawPath = GetPath(from, to);
            if (rawPath == null || rawPath.Count < 2) return null;
            List<ITile> result = new();
            int currentCost = 0;
            for (int i = 1; i < rawPath.Count; ++i)
            {
                currentCost += rawPath[i].GetMoveCost();
                if (currentCost  > limitDistance) break;
                result.Add(rawPath[i]);
            }

            return result;
        }


        private void SetMovableTiles(SetMovableTilesCommand tilesCommand, PublishContext context)
        {
            ClearSpecialTiles();
            List<(ITile, int)> movableTiles =
                hexaBoard.GetMovableAndDistanceValue(tilesCommand.CenterTile.Index, 
                    tilesCommand.Distance, 
                    tile => tile.GetMoveCost(), tile => !tile.IsPassable() || (tile != tilesCommand.CenterTile && tile.CurrentOccupant != null)).ToList();


            currentSpecialTiles = movableTiles.Select(t => t.Item1).ToList();

            foreach (var tile in movableTiles)
            {
                tile.Item1.ChangeState(ETileState.Movable, tile.Item2);
            }
        }
        private void SetSkillApplicableTiles(SetSkillApplicableTilesCommand command, PublishContext context)
        {
            ClearSpecialTiles();
            List<ITile> skillApplicableTiles = hexaBoard.GetFieldOfViewValue(command.CenterTile.Index, command.Distance, tile => tile.IsBlockVisibility()).ToList();
            currentSpecialTiles = skillApplicableTiles;
            foreach (var tile in skillApplicableTiles)
            {
                tile.ChangeState(ETileState.SkillApplicable);
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