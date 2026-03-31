using GameCore.APIGateway.Skill;
using GameCore.Domain.Skill;

namespace GameCore.Usecase.Skill
{
    public class SkillUseCase : BaseUsecase<SkillAPIGateway, SkillModel>
    {
        public SkillUseCase(SkillAPIGateway apiGateway)
        {
            _apiGateway = apiGateway;
        }
    }
}