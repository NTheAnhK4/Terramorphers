using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization;
using Sirenix.Serialization;
using Terramorphers;
using Terramorphers.Stats;

namespace UtilityAI
{
    public class Context
    {
       
       
        public Enemy Entity;
       
        public Transform Target;
        
        [OdinSerialize]
        private readonly Dictionary<string, object> data = new();

       
        public T GetData<T>(string key) => data.TryGetValue(key, out var value) ? (T)value : default;
        public void SetData(string key, object value) => data[key] = value;
        public Action UpdateContext;
       

        #region FOR ACTION

        public ITile CandidateMoveTile;
        public int CandidateSkillID;
        public ITile CandidateTileApply;


        #endregion
      
    }
}