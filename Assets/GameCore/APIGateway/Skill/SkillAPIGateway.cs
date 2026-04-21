using System.Collections.Generic;
using GameCore.Domain.Skill;
using Newtonsoft.Json;
using UnityEngine;

namespace GameCore.APIGateway.Skill
{
    public class SkillAPIGateway : BaseAPIGateway<SkillModel>
    {
        
        protected override string PlayerPrefsKey => "SkillData";
        public SkillAPIGateway(){}
     

       

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

        public bool IsSkillUnlock(int skillID) => PlayerPrefs.GetInt($"is_skill_{skillID}_unlock", 0) == 1;

        public void UnlockSkill(int skillID) =>  PlayerPrefs.SetInt($"is_skill_{skillID}_unlock",1);
    }

}
