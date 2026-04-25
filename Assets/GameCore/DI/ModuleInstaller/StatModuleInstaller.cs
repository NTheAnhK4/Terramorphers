using GameCore.Respository.Stats.Icon;
using VContainer;

namespace GameCore.DI.ModuleInstaller
{
    public class StatModuleInstaller : IModuleInstaller
    {
        public void Register(IContainerBuilder builder)
        {
            builder.RegisterSelfAsEntryPoint<StatIconRepository>();
        }
    }
}