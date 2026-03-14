
using Terramorphers;
using UnityEngine;
using VContainer;

namespace GameCore.DI.ModuleInstaller
{
    [CreateAssetMenu(fileName = "EntityInstaller", menuName = "ModuleInstaller/EntityInstaller")]
    public class EntityModuleInstaller : ScriptableObject,IModuleInstaller
    {
        [SerializeField] private EntityDatabase _entityDatabase;
        public void Register(IContainerBuilder builder)
        {
            builder.RegisterInstance(_entityDatabase);
            builder.Register<EntityManager>(Lifetime.Scoped);
            builder.Register<EntityFactory>(Lifetime.Scoped);
        }
    }

}
