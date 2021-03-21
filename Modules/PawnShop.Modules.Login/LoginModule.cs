using PawnShop.Modules.Login.Validators;
using PawnShop.Services.Implementations;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Modularity;

namespace PawnShop.Modules.Login
{
    public class LoginModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ISecretManagerService, SecretManagerService>();
            containerRegistry.RegisterSingleton<IHashService, HashService>();
            containerRegistry.RegisterSingleton<IAesService, AesService>();
            containerRegistry.RegisterSingleton<ILoginService, LoginService>();
            containerRegistry.RegisterSingleton<LoginDialogValidator>();

        }
    }
}