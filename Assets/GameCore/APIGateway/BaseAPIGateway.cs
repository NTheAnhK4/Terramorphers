using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCore.APIGateway
{
    public abstract class BaseAPIGateway<T> where T : class
    {
        protected T _model;
        protected abstract string  PlayerPrefsKey { get; }

        public T GetModel()
        {
            return _model ??= LoadFromPlayerPref();
        }

        public abstract UniTask Update(T model);

        protected virtual T LoadFromPlayerPref()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsKey)) return CreateDefaultModel();
            var json = PlayerPrefs.GetString(PlayerPrefsKey);
            try
            {
                return JsonConvert.DeserializeObject<T>(json) ?? CreateDefaultModel();
            }
            catch (Exception e)
            {
                return CreateDefaultModel();
            }
        }
        protected abstract void SaveToPlayerPref(T model);
        protected abstract T CreateDefaultModel();
    }
}