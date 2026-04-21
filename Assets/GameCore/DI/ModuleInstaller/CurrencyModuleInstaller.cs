using GameCore.APIGateway.Currency;
using GameCore.Usecase.Currency;
using VContainer;

namespace GameCore.DI.ModuleInstaller
{
    public class CurrencyModuleInstaller : IModuleInstaller
    {
        public void Register(IContainerBuilder builder)
        {
            builder.Register<CurrencyAPIGateway>(Lifetime.Singleton);
            builder.Register<CurrencyUseCase>(Lifetime.Singleton);
        }
    }
}