using System.Collections;
using System.Collections.Generic;
using GameCore.APIGateway.Level;
using GameCore.Respository.LevelData;
using GameCore.Usecase.Level;
using Terramorphers;
using UnityEngine;
using VContainer;

namespace GameCore.DI.ModuleInstaller
{
   
    public class LevelModuleInstaller :  IModuleInstaller
    {
      
        public void Register(IContainerBuilder builder)
        {
            builder.RegisterSelfAsEntryPoint<LevelRepository>();
            builder.Register<LevelAPIGateway>(Lifetime.Singleton);
            builder.Register<LevelUseCase>(Lifetime.Singleton);
        }
    }

}
