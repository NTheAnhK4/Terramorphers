using Cysharp.Threading.Tasks;
using GameCore.APIGateway.Skill;
using GameCore.Domain.Skill;

namespace GameCore.Usecase.Skill
{
    public class SkillUseCase : BaseUseCase<SkillAPIGateway, SkillModel>
    {
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
        
    }
}