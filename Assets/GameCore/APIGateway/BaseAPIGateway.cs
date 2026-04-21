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

        public virtual UniTask Update(T model)
        {
            SaveToPlayerPref(model);
            return UniTask.CompletedTask;
        }

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

        protected virtual void SaveToPlayerPref(T model)
        {
            var json = JsonConvert.SerializeObject(model);
            PlayerPrefs.SetString(PlayerPrefsKey, json);
            PlayerPrefs.Save();
        }
        protected abstract T CreateDefaultModel();
    }
}