using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameCore.Utility.Shape
{
    public partial class HexagonalGrid<T>
    {
        private Dictionary<Cube, T> m_data = new();

        private List<Cube> CubeDirectionList = new List<Cube>()
        {
            new Cube(+1, 0, -1), new Cube(+1, -1, 0), new Cube(0, -1, +1),
            new Cube(-1, 0, +1), new Cube(-1, +1, 0), new Cube(0, +1, -1),
        };

        #region Common Functions

        public HexagonalGrid()
        {
        }

        //currently for rectangal matric
        public HexagonalGrid(IList<List<T>> data, Action<T, Cube> InitIndex = null)
        {
            Cube previousCube = new Cube(0, 0, 0);
            for (int i = 0; i < data.Count; ++i)
            {
                Cube startCube;
                if (i == 0) startCube = previousCube;
                else
                {
                    if (i % 2 == 1) startCube = CubeNeighbor(previousCube, 4);
                    else startCube = CubeNeighbor(previousCube, 5);
                }

                previousCube = startCube;

                for (int j = 0; j < data[i].Count; ++j)
                {
                    if (j != 0) startCube = CubeNeighbor(startCube, 0);

                    m_data[startCube] = data[i][j];
                    InitIndex?.Invoke(data[i][j], startCube);
                }
            }
        }

        public bool IsValid(Cube cube) => m_data.ContainsKey(cube) && cube.IsValid();

        public T Get(Cube cube) => m_data[cube];

        public void Set(Cube cube, T value) => m_data[cube] = value;

        public Cube CubeDirection(int direction) => CubeDirectionList[direction];
        public Cube CubeNeighbor(Cube cube, int direction) => cube + CubeDirection(direction);

        #endregion


        #region Get Neighbors

        public IList<Cube> GetAllNeighborIndices(Cube center)
        {
            IList<Cube> result = new List<Cube>();
            for (int i = 0; i < CubeDirectionList.Count; ++i)
            {
                Cube index = CubeNeighbor(center, i);
                if (!IsValid(index)) continue;
                result.Add(index);
            }

            return result;
        }

        public IList<T> GetAllNeighborsValue(Cube center) => GetAllNeighborIndices(center).Select(Get).ToList();

        #endregion

        #region GetMovementRange

        public IList<Cube> GetMovementRangeIndices(Cube center, int distance)
        {
            IList<Cube> result = new List<Cube>();
            for (int q = -distance; q <= distance; ++q)
            {
                for (int r = Mathf.Max(-distance, -q - distance); r <= Mathf.Min(distance, -q + distance); ++r)
                {
                    int s = -q - r;
                    Cube index = center + new Cube(q, r, s);
                    if (!IsValid(index)) continue;
                    result.Add(index);
                }
            }

            return result;
        }

        public IList<T> GetMovementRangeValue(Cube center, int distance) => GetMovementRangeIndices(center, distance).Select(Get).ToList();

        public IList<(Cube, int)> GetMovementRangeIndicesAndDistance(Cube center, int distance)
        {
            IList<Cube> cubes = GetMovementRangeIndices(center, distance);
            IList<(Cube, int)> result = cubes.Select(t => (t, Mathf.RoundToInt(Cube.Distance(t, center)))).ToList();
            return null;
        }

        public IList<(T, int)> GetMovementRangeValueAndDistance(Cube center, int distance) =>
            GetMovementRangeIndicesAndDistance(center, distance).Select(t => (Get(t.Item1), t.Item2)).ToList();

        #endregion

        #region Get Path

        public IList<Cube> GetLinedPathIndices(Cube a, Cube b)
        {
            float distance = Cube.Distance(a, b);
            IList<Cube> results = new List<Cube>();
            for (int i = 0; i <= distance; ++i)
            {
                var cube = Cube.Round(Cube.Lerp(a, b, 1.0f / distance * i));
                if(!IsValid(cube)) continue;
                results.Add(cube);
            }

            return results;
        }

        public IList<T> GetLinedPathValue(Cube a, Cube b) => GetLinedPathIndices(a, b).Select(Get).ToList();

       
        #endregion

       
    }
}