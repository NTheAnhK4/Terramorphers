using Cysharp.Threading.Tasks;
using GameCore.Domain.Level;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCore.APIGateway.Level
{
    public class LevelAPIGateway : BaseAPIGateway<LevelModel>
    {
        protected override string PlayerPrefsKey => "LevelData";
        public override UniTask Update(LevelModel model)
        {
            SaveToPlayerPref(model);
            return UniTask.CompletedTask;
        }

        protected override void SaveToPlayerPref(LevelModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            PlayerPrefs.SetString(PlayerPrefsKey, json);
            PlayerPrefs.Save();
        }

        protected override LevelModel CreateDefaultModel()
        {
            return new LevelModel(0) { };
        }

        public int GetCurrentStageOfLevel(int level)
        {
            return PlayerPrefs.GetInt($"current_stage_of_level_{level}",0);
        }

        public void SetCurrentStageOfLevel(int level, int value)
        {
            PlayerPrefs.SetInt($"current_stage_of_level_{level}", value);
        }

        public int GetStars(int level, int stage)
        {
            return PlayerPrefs.GetInt($"stars_of_level_{level}_stage_{stage}", 0);
        }

        public void SetStars(int level, int stage, int stars)
        {
            PlayerPrefs.SetInt($"stars_of_level_{level}_stage_{stage}", stars);
        }
    }
}