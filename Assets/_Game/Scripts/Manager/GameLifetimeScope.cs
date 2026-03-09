

using CoreGame;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VitalRouter.VContainer;

namespace Terramorphers
{
    public class GameLifetimeScope : LifetimeScope
    {
       
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterComponentInHierarchy<GameManager>();
            builder.Register<AdvantureGameState>(Lifetime.Singleton);
            builder.Register<WinState>(Lifetime.Singleton);
            builder.RegisterVitalRouter(routing =>
            {
                routing.MapEntryPoint<GameFSM>();
            });
        }
    }

}

