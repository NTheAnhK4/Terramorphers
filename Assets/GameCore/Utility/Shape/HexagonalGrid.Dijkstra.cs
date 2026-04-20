using System;
using System.Collections.Generic;

using System.Linq;
using GameCore.Utility.DataStructure;
using UnityEngine;

namespace GameCore.Utility.Shape
{
    public partial class HexagonalGrid<T>
    {
        
        public IList<(Cube, int)> GetMovableAndDistanceIndices(Cube center, int distance, Func<T, int> moveCost, Predicate<T> isBlock)
        {
            var result = new List<(Cube, int)>();
            PriorityQueue<(Cube, int)> priorityQueue = new PriorityQueue<(Cube, int)>((a, b) => a.Item2.CompareTo(b.Item2));
            Dictionary<Cube, int> D = new Dictionary<Cube, int>();

            D[center] = 0;
            HashSet<Cube> visisted = new();
            priorityQueue.Enqueue((center,0));
           
            while (priorityQueue.Count > 0)
            {
                var currentNode = priorityQueue.Dequeue();
               
                if(visisted.Contains(currentNode.Item1)) continue;
                visisted.Add(currentNode.Item1);

                foreach (var neighbor in GetAllNeighborIndices(currentNode.Item1))
                {
                    
                    if(isBlock(Get(neighbor))) continue;
                    int newCost = D[currentNode.Item1] + moveCost(Get(neighbor));
                    if(newCost > distance) continue;
                    if (!D.ContainsKey(neighbor) || D[neighbor] > newCost)
                    {
                        D[neighbor] = newCost;
                        priorityQueue.Enqueue((neighbor, D[neighbor]));
                    }
                }
            }

            foreach (var (key, val) in D)
            {
                if(key == center) continue;
                result.Add((key,val));
            }

            return result;
        }

        public IList<(T, int)> GetMovableAndDistanceValue(Cube center, int distance, Func<T, int> moveCost, Predicate<T> isBlock)
            => GetMovableAndDistanceIndices(center, distance, moveCost, isBlock).Select(a => (Get(a.Item1),a.Item2)).ToList();
        
    }
}