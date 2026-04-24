using GameCore.DI.ModuleInstaller;
using GameCore.Presentation.Shared;

using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZBase.UnityScreenNavigator.Core;

namespace GameCore.DI
{
    public class AppLifetimeScope : LifetimeScope
    {
        [SerializeField] private UnityScreenNavigatorLauncher launcher;
        
      
     
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterModuleInstaller<RouterModuleInstaller>();
            builder.Register<TransitionService>(Lifetime.Singleton);
            builder.RegisterModuleInstaller<SkillModuleInstaller>();
            builder.RegisterModuleInstaller<TileModuleInstaller>();
            builder.RegisterModuleInstaller<LevelModuleInstaller>();
            builder.RegisterModuleInstaller<EntityModuleInstaller>();
            builder.RegisterModuleInstaller<QuestModuleInstaller>();
            builder.RegisterModuleInstaller<RewardModuleInstaller>();
            builder.RegisterModuleInstaller<CurrencyModuleInstaller>();
            builder.RegisterModuleInstaller<AudioModuleInstaller>();
            builder.RegisterComponent(launcher);
        }
    }

}
