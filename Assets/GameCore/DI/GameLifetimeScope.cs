


using GameCore.DI.ModuleInstaller;

using VContainer;
using VContainer.Unity;
using VitalRouter.VContainer;
using Terramorphers;
namespace GameCore.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
       
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterComponentInHierarchy<GameManager>();
            builder.RegisterComponentInHierarchy<BoardManager>();
            
            builder.Register<InputManager>(Lifetime.Scoped);
            
           

            builder.Register<LoadingState>(Lifetime.Scoped);
            builder.Register<AdvantureState>(Lifetime.Scoped);
            builder.Register<WinState>(Lifetime.Scoped);
           
           
            builder.RegisterVitalRouter(routing =>
            {
                routing.MapEntryPoint<GameFSM>();
            });
        }
    }

}

