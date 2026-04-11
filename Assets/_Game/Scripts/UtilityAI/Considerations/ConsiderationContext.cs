using System;
using System.Collections.Generic;
using GameCore.Utility;
using UnityEngine;

namespace UtilityAI.Considerations
{
    public enum EContextType
    {
        None = 0,
        Self = 1,
        Tile = 2,
        Enemy = 3,
        Ally = 4,
        Skill = 5,
        Target = 6,
    }
    public class ConsiderationContext
    {
        private Dictionary<EContextType, Context> data = new();
        private Dictionary<EContextType, object[]> paramData = new();
        public void Set(EContextType key, Context value) => data[key] = value;
        public Context Get(EContextType key) => data[key];
        public bool Has(EContextType key) => data.ContainsKey(key);
        public void SetParams(EContextType key, params object[] value) =>   paramData[key] = value;

        public object[] GetParams(EContextType key) => paramData[key];
        public bool HasParamKey(EContextType key) => paramData.ContainsKey(key);
    }
}