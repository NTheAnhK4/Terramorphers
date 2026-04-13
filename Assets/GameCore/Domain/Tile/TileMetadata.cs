using System;
using UnityEngine;

namespace GameCore.Domain.Tile
{
    [Serializable]
    public class TileMetadata
    {
        [SerializeField] private ETileType type;
        [SerializeField] private string tileName;
        [SerializeField] private string description;
        [SerializeField] private string addresable;

        public ETileType Type => type;

        public string TileName => tileName;

        public string Description => description;

        public string Addresable => addresable;
    }
}