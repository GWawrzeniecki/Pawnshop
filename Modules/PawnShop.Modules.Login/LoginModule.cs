using PawnShop.Core.Attributes;
using PawnShop.Modules.Login.Validators;
using PawnShop.Services.Implementations;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Modularity;

namespace PawnShop.Modules.Login
{
    [Privilege("Login")]
    [Order(0)]
    public class LoginModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ISecretManagerService, SecretManagerService>();
            containerRegistry.Register<IEnvironmentVariableService, EnvironmentVariableService>();
            containerRegistry.Register<IHashService, HashService>();
            containerRegistry.Register<IAesService, AesService>();
            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<LoginDialogValidator>();
        }
    }
}