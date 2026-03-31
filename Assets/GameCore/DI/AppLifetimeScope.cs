using GameCore.DI.ModuleInstaller;
using GameCore.Presentaion.Shared;

using UnityEngine;
using VContainer;
using VContainer.Unity;
using ZBase.UnityScreenNavigator.Core;

namespace GameCore.DI
{
    public class AppLifetimeScope : LifetimeScope
    {
        [SerializeField] private UnityScreenNavigatorLauncher launcher;
        [SerializeField] private TileModuleInstaller _tileModuleInstaller;
        [SerializeField] private LevelModuleInstaller _levelModuleInstaller;
        [SerializeField] private EntityModuleInstaller _entityModuleInstaller;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterModuleInstaller<RouterModuleInstaller>();
            builder.Register<TransitionService>(Lifetime.Singleton);
            builder.RegisterModuleInstaller<SkillModuleInstaller>();
            _tileModuleInstaller.Register(builder);
            _levelModuleInstaller.Register(builder);
            _entityModuleInstaller.Register(builder);
            builder.RegisterComponent(launcher);
        }
    }

}
