using System;
using System.Collections.Generic;
using System.Linq;
using CoreGame;
using System.IO;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using GameCore.Utility.Shape;

using Terramorphers.Command;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UtilityAI;
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

        public HexagonalGrid<ITile> HexaBoard => hexaBoard;

       
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
            bags.Add(_subscribable.Subscribe<EntityTileDistCommand>(CaculateEntityTileDistance));
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

        public List<(ITile, int)> GetMovableTiles(ITile center, int distance)
        {
            if (distance == 0) return new List<(ITile, int)>();
            return hexaBoard.GetMovableAndDistanceValue(
                center.Index, 
                distance, 
                tile => tile.GetMoveCost(),
                tile => !tile.IsPassable() || (tile != center && tile.CurrentOccupant != null)).ToList();
        }

        
        private void SetMovableTiles(SetMovableTilesCommand tilesCommand, PublishContext context)
        {
            ClearSpecialTiles();
            List<(ITile, int)> movableTiles = GetMovableTiles(tilesCommand.CenterTile, tilesCommand.Distance);
            


            currentSpecialTiles = movableTiles.Select(t => t.Item1).ToList();

            foreach (var tile in movableTiles)
            {
                tile.Item1.ChangeState(ETileState.Movable, tile.Item2);
            }
        }
        private void SetSkillApplicableTiles(SetSkillApplicableTilesCommand command, PublishContext context)
        {
            ClearSpecialTiles();
            List<ITile> skillApplicableTiles = hexaBoard.GetFieldOfViewValue(command.Entity.CurrentTile.Index, command.Distance, tile => tile.IsBlockVisibility()).ToList();
            currentSpecialTiles = skillApplicableTiles;
            bool hasTileTarget = command.SkillTargetTypes.Contains(ESkillTargetType.Tile);
            bool canSelf  = command.SkillTargetTypes.Contains(ESkillTargetType.Self);
            bool canAlly  = command.SkillTargetTypes.Contains(ESkillTargetType.Ally);
            bool canEnemy = command.SkillTargetTypes.Contains(ESkillTargetType.Enemy);
            foreach (var tile in skillApplicableTiles)
            {
              
                if (hasTileTarget)
                {
                    tile.ChangeState(ETileState.TileTargetSkill);
                    continue;
                }

                var occupant = tile.CurrentOccupant;

                if (occupant == null)
                {
                    tile.ChangeState(ETileState.SkillApplicable);
                    continue;
                }

                ETileState state = ETileState.SkillApplicable;

                if (occupant == command.Entity)
                {
                    if (canSelf) state = ETileState.SelfTargetSkill;
                }
                else if (occupant.TeamID == command.Entity.TeamID)
                {
                    if (canAlly) state = ETileState.AllyTargetSkill;
                }
                else
                {
                    if (canEnemy) state = ETileState.EnemyTargetSkill;
                }

                tile.ChangeState(state);
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
        private void CaculateEntityTileDistance(EntityTileDistCommand command, PublishContext context)
        {
            List<(ITile, int)> movableTiles =hexaBoard.GetMovableAndDistanceValue(
                command.Tile.Index, 
                100, 
                tile => tile.GetMoveCost(),
                tile => !tile.IsPassable() || (tile == command.Tile)).ToList(); 
            foreach (var item in movableTiles)
            {
                string key = string.Format(BlackBoardConstant.ENTITY_TO_TILE_DISTANCE_KEY, command.Name);
                item.Item1.Context.SetData(key, item.Item2);
            }
        }
    }
}