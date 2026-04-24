using GameCore.APIGateway.Audio;
using GameCore.Usecase.Audio;
using VContainer;

namespace GameCore.DI.ModuleInstaller
{
    public class AudioModuleInstaller : IModuleInstaller
    {
        public void Register(IContainerBuilder builder)
        {
            builder.Register<AudioAPIGateway>(Lifetime.Singleton);
            builder.Register<AudioUseCase>(Lifetime.Singleton);
        }
    }
}