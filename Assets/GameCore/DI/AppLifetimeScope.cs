using GameCore.DI.ModuleInstaller;
using VContainer;
using VContainer.Unity;

namespace GameCore.DI
{
    public class AppLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterModuleInstaller<RouterModuleInstaller>();

        }
    }

}
