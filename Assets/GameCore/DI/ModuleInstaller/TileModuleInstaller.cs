using Terramorphers;
using UnityEngine;
using VContainer;

namespace GameCore.DI.ModuleInstaller
{
    [CreateAssetMenu(fileName = "TileInstaller", menuName = "ModuleInstaller/TileInstaller")]
    public class TileModuleInstaller : ScriptableObject, IModuleInstaller
    {
        [SerializeField] private TileDatabase _tileDatabase;
        public void Register(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tileDatabase);
            builder.Register<TileFactory>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}