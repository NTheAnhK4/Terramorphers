

using UnityEngine;
using System;
using System.Collections.Generic;
using GameCore.Domain.Tile;


namespace Terramorphers
{
    [Serializable]
    public class LevelMetadata
    {
        [SerializeField] private string boardDataAddressable;

        public string BoardDataAddressable => boardDataAddressable;
    }
    [Serializable]
    public class BoardData
    {
        public List<BoardRow> Rows;
    }

    [Serializable]
    public class BoardRow
    {
        public List<ETileType> Tiles;
    }

}
