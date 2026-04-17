
using GameCore.Respository.Entity;
using Terramorphers;

using VContainer;

namespace GameCore.DI.ModuleInstaller
{
    
    public class EntityModuleInstaller : IModuleInstaller
    {
       
        public void Register(IContainerBuilder builder)
        {
            builder.RegisterSelfAsEntryPoint<EntityRepository>();
           
            builder.Register<EntityManager>(Lifetime.Scoped);
            builder.Register<EntityFactory>(Lifetime.Scoped);
        }
    }

}
