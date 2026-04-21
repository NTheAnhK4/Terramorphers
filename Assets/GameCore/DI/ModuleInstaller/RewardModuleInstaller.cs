using GameCore.Respository.Reward;
using VContainer;

namespace GameCore.DI.ModuleInstaller
{
    public class RewardModuleInstaller : IModuleInstaller
    {
        public void Register(IContainerBuilder builder)
        {
            builder.RegisterSelfAsEntryPoint<RewardItemRepository>();
        }
    }
}