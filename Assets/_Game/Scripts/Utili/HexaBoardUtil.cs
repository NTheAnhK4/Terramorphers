using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Terramorphers
{
    public static class HexaBoardUtil
    {
        #region Basic

        static readonly Vector2Int[] evenDirs =
        {
            new(-1, 0), new(1, 0),
            new(-1, 1), new(1, 1),
            new(0, -1), new(0, 1)
        };

        static readonly Vector2Int[] oddDirs =
        {
            new(-1, -1), new(1, -1),
            new(-1, 0), new(1, 0),
            new(0, -1), new(0, 1)
        };

        public static Vector2Int GetIndex<T>(this List<List<T>> board, T value) where T : class
        {
            for (int i = 0; i < board.Count; ++i)
            {
                for (int j = 0; j < board[i].Count; ++j)
                {
                    if (board[i][j] == value) return new Vector2Int(i, j);
                }
            }

            return new Vector2Int(-1, -1);
        }

        public static List<Vector2Int> GetNeighborIndices<T>(this List<List<T>> board, Vector2Int center)
        {
            List<Vector2Int> result = new();

            var dirs = center.x % 2 == 0 ? evenDirs : oddDirs;

            foreach (var d in dirs)
            {
                int x = center.x + d.x;
                int y = center.y + d.y;

                Vector2Int index = new Vector2Int(x, y);
                if (board.IsValidPos(index)) result.Add(new Vector2Int(x, y));
            }

            return result;
        }

        public static T Get<T>(this List<List<T>> board, Vector2Int index)
        {
            return board[index.x][index.y];
        }

        public static void Set<T>(this List<List<T>> board, Vector2Int index, T value)
        {
            board[index.x][index.y] = value;
        }

       

        public static bool IsValidPos<T>(this List<List<T>> board, Vector2Int pos)
        {
            if (pos.x < 0 || pos.x >= board.Count) return false;
            if (pos.y < 0 || pos.y >= board[pos.x].Count) return false;
            return true;
        }

        #endregion

        public static List<(T,int)> GetTileMovable<T>(this List<List<T>> board, T center, int radius, Func<T, int> getMoveCost) where T : class
        {
            var centerID = board.GetIndex(center);
            if (!board.IsValidPos(centerID)) return new();
            HashSet<Vector2Int> visited = new();
            Queue<Vector2Int> queue = new();
            List<List<int>> distance = board
                .Select(row => Enumerable.Repeat(int.MaxValue, row.Count).ToList())
                .ToList();

            distance.Set(centerID,0);
            distance[centerID.x][centerID.y] = 0;
            queue.Enqueue(centerID);
            visited.Add(centerID);

            List<(T,int)> result = new();
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                var neighborIndices = board.GetNeighborIndices(node);
                foreach (var neighborIndex in neighborIndices)
                {
                    if (!board.IsValidPos(neighborIndex) || visited.Contains(neighborIndex)) continue;
                    visited.Add(neighborIndex);
                    int moveCost = getMoveCost(board.Get(neighborIndex));
                    if (moveCost < 0) continue;

                    int newDistance = distance.Get(node) + moveCost;
                    distance.Set(neighborIndex, newDistance);
                    if(newDistance <= radius) result.Add((board.Get(neighborIndex), newDistance));
                    if(newDistance < radius) queue.Enqueue(neighborIndex);
                    

                }
            }

            return result;
        }


        public static List<T> GetPath<T>(this List<List<T>> board, T from, T to, Func<T, bool> isPassable) where T : class
        {
            var fromID = board.GetIndex(from);
            var toID = board.GetIndex(to);

            if (fromID == toID) return new List<T>();

            if (!board.IsValidPos(fromID) || !board.IsValidPos(toID))
                return null;

            if (!isPassable(from))
                return null;

            HashSet<Vector2Int> visited = new();
            
            Dictionary<Vector2Int, Vector2Int> trace = new();
            Queue<Vector2Int> queue = new();

            visited.Add(fromID);
            queue.Enqueue(fromID);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                foreach (var neighbor in board.GetNeighborIndices(node))
                {
                    if (!board.IsValidPos(neighbor)) continue;
                    if (visited.Contains(neighbor)) continue;

                    var tile = board[neighbor.x][neighbor.y];
                    if (!isPassable(tile)) continue;

                    visited.Add(neighbor);
                    trace[neighbor] = node;
                    queue.Enqueue(neighbor);

                    if (neighbor == toID)
                    {
                        List<T> path = new();
                        var cur = toID;

                        while (true)
                        {
                            path.Add(board[cur.x][cur.y]);
                            if (cur == fromID) break;
                            cur = trace[cur];
                        }

                        path.Reverse();
                        return path;
                    }
                }
            }

            return null;
        }
    }
}