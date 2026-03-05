using VContainer;

namespace GameCore.DI
{
    public interface IModuleInstaller
    {
        void Register(IContainerBuilder builder);
    }
}