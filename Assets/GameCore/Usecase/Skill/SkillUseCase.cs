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
    }
}