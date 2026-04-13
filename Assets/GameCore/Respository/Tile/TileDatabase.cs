using System;
using System.Collections.Generic;
using GameCore.Domain.Shared;
using GameCore.Domain.Tile;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GameCore.Respository.Tile
{
    [CreateAssetMenu(fileName = "TileDatabase", menuName = "Database/TileDatabase")]
    public class TileDatabase : BaseDatabase<ETileType, TileMetadata>, ITileDatabase
    {
        #if UNITY_EDITOR
        [Button]
        private void Bake()
        {
            Dictionary<ETileType, TileMetadata> dataCache = new Dictionary<ETileType, TileMetadata>(Datas);
            foreach(ETileType tileType in Enum.GetValues(typeof(ETileType)))
            {
                if (dataCache.ContainsKey(tileType)) continue;
                else Datas[tileType] = new TileMetadata();
            }
            SetSerializedElements(Datas);
            EditorUtility.SetDirty(this);
        }
        #endif
    }
}