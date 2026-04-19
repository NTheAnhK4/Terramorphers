using System.Collections.Generic;
using GameCore.Domain.Skill;
using R3;
using UnityEngine;

namespace Terramorphers.Skill
{
    public class SkillSystem
    {
        private ISkillRepository _skillRepository;

        private ISkillDatabase skillDatabase;
        private List<int> _skillIDs;
        private TerramorphersEntity _entity;
      
        private bool initialized;

        public SkillSystem(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }
        public void Init(TerramorphersEntity entity,List<int> skillIDs)
        {
            _skillIDs = skillIDs;
            _entity = entity;
            initialized = false;
            foreach (var skillID in skillIDs)
            {
                _entity.DataCache.SkillCoolDown.TryAdd(skillID, new ReactiveProperty<int>());
            }
        }

        private void Initialized()
        {
            initialized = true;
            skillDatabase = _skillRepository.Get();
            foreach (var skillID in _skillIDs)
            {
                var skillMetadata = skillDatabase.GetByType(skillID);
                int coolDown = 0;
                if (skillMetadata.PrecoolDown > 0) coolDown = skillMetadata.PrecoolDown;

                _entity.DataCache.SkillCoolDown[skillID].Value = coolDown;
            }
        }

        public void OnEnter()
        {
            if (!initialized) Initialized();
            else
            {
                foreach (var skillID in _skillIDs)
                {
                    ReactiveProperty<int> skillCoolDown = _entity.DataCache.SkillCoolDown[skillID];
                    if(skillCoolDown.Value == 0) continue;
                    int newValue = Mathf.Max(0, skillCoolDown.Value - 1);
                    SetSkillCoolDown(skillID, newValue);
               
               
                }
            }
            
        }

        private void SetSkillCoolDown(int skillID, int value)
        {
    
            _entity.DataCache.SkillCoolDown[skillID].Value = value;
        }

        public SkillMetadata GetSkillMetadata(int skillID)
        {
            if(!initialized) Initialized();
            return skillDatabase.GetByType(skillID);
        }

        public void UseSkill(int skillID)
        {
           
            var skillMetadata = GetSkillMetadata(skillID); 
            SetSkillCoolDown(skillID, skillMetadata.CoolDown);
          
           
        }
    }
}