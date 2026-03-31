using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Domain.Skill;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCore.APIGateway.Skill
{
    public class SkillAPIGateway : BaseAPIGateway<SkillModel>
    {
        protected override string PlayerPrefsKey => "SkillData";
        public SkillAPIGateway(){}
        public override UniTask Update(SkillModel model)
        {
            SaveToPlayerPref(model);
            return UniTask.CompletedTask;
        }

       

        protected override void SaveToPlayerPref(SkillModel model)
        {
            var json = JsonConvert.SerializeObject(model);
            PlayerPrefs.SetString(PlayerPrefsKey, json);
            PlayerPrefs.Save();
        }

        protected override SkillModel CreateDefaultModel()
        {
            return new SkillModel()
            {
                CurrentSkills = new List<int>() { 0,1,2,3}
            };
        }
    }

}
