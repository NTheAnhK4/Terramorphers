using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore.Utility.DataStructure;
using UnityEngine;

namespace GameCore.Utility.Shape
{
    public partial class HexagonalGrid<T>
    {
        class NodeAStar
        {
            public Cube Value;
            public NodeAStar PreviousNodeAStar;
            public int gCost, hCost;
            public int fCost => gCost + hCost;

            public NodeAStar(Cube Value, int gCost, int hCost, NodeAStar previousNodeAStar)
            {
                this.Value = Value;
                this.hCost = hCost;
                this.gCost = gCost;
                this.PreviousNodeAStar = previousNodeAStar;
            }


            public static int Compare(NodeAStar a, NodeAStar b)
            {
                if (a.fCost < b.fCost) return -1;
                else if (a.fCost > b.fCost) return 1;
                return a.hCost.CompareTo(b.hCost);
            }
        }


        public IList<Cube> GetPathIndices(Cube from, Cube to, Func<T, int> moveCost, Predicate<T> isBlock)
        {
            IList<Cube> GetPath(NodeAStar currentNode)
            {
                IList<Cube> result = new List<Cube>();
                result.Add(currentNode.Value);
                while (currentNode.PreviousNodeAStar != null)
                {
                    currentNode = currentNode.PreviousNodeAStar;
                    result.Add(currentNode.Value);
                }

                result = result.Reverse().ToList();
                return result;
            }

            NodeAStar GetBestNode(List<NodeAStar> nodes)
            {
                NodeAStar result = null;
                foreach (var node in nodes)
                {
                    if (result == null) result = node;
                    else
                    {
                        if (NodeAStar.Compare(node, result) < 0) result = node;
                    }
                }

                return result;
            }


            if (isBlock(Get(from)) || isBlock(Get(to))) return null;
            if (from == to) return new List<Cube>();

            List<NodeAStar> nodes = new List<NodeAStar>();

            var fromNode = new NodeAStar(from, 0, (int)Cube.Distance(from, to), null);
            nodes.Add(fromNode);


            Dictionary<Cube, NodeAStar> visisted = new();

            visisted[from] = fromNode;
            HashSet<NodeAStar> openSet = new();
            openSet.Add(fromNode);

            while (nodes.Count > 0)
            {
                var currentNode = GetBestNode(nodes);
                nodes.Remove(currentNode);
                openSet.Remove(currentNode);

                if (currentNode.Value == to) return GetPath(currentNode);
                foreach (var neighbor in GetAllNeighborIndices(currentNode.Value))
                {
                    if (isBlock(Get(neighbor))) continue;

                    int gCost = currentNode.gCost + moveCost(Get(neighbor));
                    int hCost = (int)Cube.Distance(neighbor, to);

                    if (visisted.TryGetValue(neighbor, out var neighborNode))
                    {
                        if (gCost < neighborNode.gCost)
                        {
                            neighborNode.gCost = gCost;
                            neighborNode.hCost = hCost;
                            neighborNode.PreviousNodeAStar = currentNode;
                            if (!openSet.Contains(neighborNode))
                            {
                                nodes.Add(neighborNode);
                                openSet.Add(neighborNode);
                            }
                        }
                    }
                    else
                    {
                        NodeAStar newNodeAStar = new NodeAStar(neighbor, gCost, hCost, currentNode);
                        visisted[neighbor] = newNodeAStar;
                        nodes.Add(newNodeAStar);
                        openSet.Add(newNodeAStar);
                    }
                }
            }

            return null;
        }

        public IList<T> GetPathValue(Cube from, Cube to, Func<T, int> moveCost, Predicate<T> isBlock)
        {
            var indices = GetPathIndices(from, to, moveCost, isBlock);
            if (indices == null) return null;
            return indices.Select(Get).ToList();
        }
    }
}