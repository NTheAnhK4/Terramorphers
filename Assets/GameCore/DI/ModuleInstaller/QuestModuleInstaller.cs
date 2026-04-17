using GameCore.APIGateway.Quest;
using GameCore.Respository.Quest;
using GameCore.Usecase.Quest;
using VContainer;

namespace GameCore.DI.ModuleInstaller
{
    public class QuestModuleInstaller : IModuleInstaller
    {
        public void Register(IContainerBuilder builder)
        {
            builder.RegisterSelfAsEntryPoint<QuestRepository>();
            builder.Register<QuestAPIGateway>(Lifetime.Singleton);
            builder.Register<QuestUseCase>(Lifetime.Singleton);
        }
    }
}