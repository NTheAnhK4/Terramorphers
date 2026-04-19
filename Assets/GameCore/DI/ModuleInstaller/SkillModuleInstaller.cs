using GameCore.APIGateway.Skill;
using GameCore.Respository.Skill;
using GameCore.Usecase.Skill;
using Terramorphers.Skill;
using VContainer;

namespace GameCore.DI.ModuleInstaller
{
    public class SkillModuleInstaller : IModuleInstaller
    {
        public void Register(IContainerBuilder builder)
        {
            builder.RegisterSelfAsEntryPoint<SkillRepository>();
            builder.Register<SkillAPIGateway>(Lifetime.Singleton);
            builder.Register<SkillUseCase>(Lifetime.Singleton);
            builder.Register<SkillSystem>(Lifetime.Transient);
        }
    }
}