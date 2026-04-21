using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.APIGateway.Skill;
using GameCore.Domain.Skill;

namespace GameCore.Usecase.Skill
{
    public class SkillUseCase : BaseUseCase<SkillAPIGateway, SkillModel>
    {
        private List<int> defaultUnlockedSkillIds = new List<int>() { 0, 1, 2, 3 };
        public SkillUseCase(SkillAPIGateway apiGateway)
        {
            _apiGateway = apiGateway;
        }

        public void AddSkill(SkillModel model, int skillID)
        {
            model.CurrentSkills.Add(skillID);
            Update(model).Forget();
        }

        public void RemoveSkill(SkillModel model, int skillID)
        {
            model.CurrentSkills.Remove(skillID);
            Update(model).Forget();
        }

        public bool IsSkillUnlock(int skillID)
        {
            if (defaultUnlockedSkillIds.Contains(skillID)) return true;
            return _apiGateway.IsSkillUnlock(skillID);
        }

        public void UnlockSkill(int skillID) => _apiGateway.UnlockSkill(skillID);

    }
}