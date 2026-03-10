using System.Collections;
using System.Collections.Generic;
using Terramorphers;
using UnityEngine;
using VContainer;

namespace GameCore.DI.ModuleInstaller
{
    [CreateAssetMenu(fileName = "LevelInstaller", menuName = "ModuleInstaller/LevelInstaller")]
    public class LevelModuleInstaller : ScriptableObject, IModuleInstaller
    {
        [SerializeField] private LevelDatabase _levelDatabase;
        public void Register(IContainerBuilder builder)
        {
            builder.RegisterInstance(_levelDatabase);
        }
    }

}
