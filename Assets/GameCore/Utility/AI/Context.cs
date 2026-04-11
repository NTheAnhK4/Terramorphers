using System.Collections.Generic;
using UnityEngine;

using System;

using Sirenix.Serialization;


namespace GameCore.Utility
{
    public class Context
    {
        [OdinSerialize]
        private readonly Dictionary<string, object> data = new();

       
        public T GetData<T>(string key) => data.TryGetValue(key, out var value) ? (T)value : default;
        public void SetData(string key, object value) => data[key] = value;
        public bool HasData(string key) => data.ContainsKey(key);
        public Action UpdateContext;
       

      
    }
}