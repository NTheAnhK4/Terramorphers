using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace GameCore.Utility.Shape
{
    public partial class HexagonalGrid<T>
    {
        // tmp r^3
        public bool IsCubeVisible(Cube from, Cube target, Predicate<T> isBlockVisible)
        {
            List<Cube> path = GetLinedPathIndices(from, target).ToList();
            if (path.Count <= 2) return true;
            path.Remove(from);
            path.Remove(target);
            foreach (var point in path)
            {
                if (isBlockVisible(Get(point))) return false;
            }

            return true;
        }

        public IList<Cube> GetFieldOfViewIndices(Cube from, int distance, Predicate<T> isBlockVisible)
        {
            if (distance <= 0) return new List<Cube>();
            IList<Cube> result = new List<Cube>();
            Queue<Cube> queue = new();
          
            Dictionary<Cube, int> D = new();
            queue.Enqueue(from);
            result.Add(from);
            D[from] = 0;
            while (queue.Count > 0)
            {
                Cube currentNode = queue.Dequeue();
                if(D[currentNode] == distance) continue;
                
                foreach (var neighbor in GetAllNeighborIndices(currentNode))
                {
                    if (D.ContainsKey(neighbor)) continue;
                    if(!IsCubeVisible(from, neighbor,isBlockVisible)) continue;
                    D[neighbor] = D[currentNode] + 1;
                    if (D[neighbor] > distance) continue;
                    result.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }

            return result;
        }

        public IList<T> GetFieldOfViewValue(Cube from, int distance, Predicate<T> isVisible) => GetFieldOfViewIndices(from, distance, isVisible).Select(Get).ToList();
    }
}
