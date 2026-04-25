using System;
using GameCore.Domain.Shared;
using GameCore.Domain.Stats;
using GameCore.Domain.Stats.Icon;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GameCore.Respository.Stats.Icon
{
    [CreateAssetMenu(fileName = "StatIconDatabase", menuName = "Database/StatIconDatabase")]
    public class StatIconDatabase : BaseDatabase<EStatsType,StatIconMetadata>, IStatIconDatabase
    {
       
        [Button]
        private void Bake()
        {
            foreach (EStatsType statType in Enum.GetValues(typeof(EStatsType)))
            {
                if(Datas.ContainsKey(statType)) continue;
                Datas.Add(statType, new StatIconMetadata());
            }
#if UNITY_EDITOR
            SetSerializedElements(Datas);
            EditorUtility.SetDirty(this);
#endif
        }
      
    }
}