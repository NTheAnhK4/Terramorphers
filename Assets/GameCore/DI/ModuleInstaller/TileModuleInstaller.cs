using GameCore.Respository.Tile;
using Terramorphers;

using VContainer;

namespace GameCore.DI.ModuleInstaller
{
  
    public class TileModuleInstaller : IModuleInstaller
    {
      
        public void Register(IContainerBuilder builder)
        {
            builder.RegisterSelfAsEntryPoint<TileRepository>();
            builder.Register<TileFactory>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}