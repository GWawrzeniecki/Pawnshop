using AutoMapper;
using MahApps.Metro.Controls;
using PawnShop.Controls.BaseTaskbar.Views;
using PawnShop.Core;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Core.SharedVariables;
using PawnShop.Core.Taskbar;
using PawnShop.Dialogs.Views;
using PawnShop.Dialogs.ViewsModels;
using PawnShop.Mapper.Profiles;
using PawnShop.Modules.Contract;
using PawnShop.Modules.Home;
using PawnShop.Modules.Login;
using PawnShop.Modules.Login.ViewModels;
using PawnShop.Modules.Login.Views;
using PawnShop.Services.DataService;
using PawnShop.Services.Implementations;
using PawnShop.Services.Interfaces;
using PawnShop.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows;
using System.Windows.Controls;

namespace PawnShop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
            containerRegistry.Register<IUnitOfWork, UnitOfWork>();
            containerRegistry.RegisterSingleton<IUIService, UIService>();
            containerRegistry.RegisterSingleton<IShellService, ShellService>();
            containerRegistry.RegisterSingleton<ISessionContext, SessionContext>();
            containerRegistry.RegisterSingleton<IValidatorService, ValidatorService>();
            containerRegistry.RegisterSingleton<IClientService, ClientService>(); // to do (maybe) to move in future
            containerRegistry.RegisterSingleton<IContractItemService, ContractItemService>();
            containerRegistry.RegisterDialogWindow<MahappsDialogWindow>();
            containerRegistry.RegisterDialog<LoginDialog, LoginDialogViewModel>();
            containerRegistry.RegisterDialog<NotificationDialog, NotificationDialogViewModel>();
            ConfigureMapper(containerRegistry);
        }

        private void ConfigureMapper(IContainerRegistry containerRegistry)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AddClientViewModelToClientProfile>();
                cfg.AddProfile<AddContractItemDialogViewModelToContractItemProfile>();
            });
            var mapper = configuration.CreateMapper();

            containerRegistry.RegisterInstance(typeof(IMapper), mapper);
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            regionAdapterMappings.RegisterMapping<StackPanel>(Container.Resolve<StackPanelRegionAdapter>());
            regionAdapterMappings.RegisterMapping<HamburgerMenuItemCollection>(
                Container.Resolve<HamburgerMenuItemCollectionRegionAdapter>());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<LoginModule>();
            moduleCatalog.AddModule<HomeModule>(InitializationMode.OnDemand);
            moduleCatalog.AddModule<ContractModule>(InitializationMode.OnDemand);
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            regionBehaviors.AddIfMissing(DependentViewRegionBehavior.BehaviorKey, typeof(DependentViewRegionBehavior));
            regionBehaviors.AddIfMissing(RegionManagerAwareBehavior.BehaviorKey, typeof(RegionManagerAwareBehavior));


            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
        }

        protected override void OnInitialized()
        {
            #region Login

            var loginService = Container.Resolve<ILoginService>();
            var moduleManager = Container.Resolve<IModuleManager>();
            var result = loginService.ShowLoginDialog();

            if (result != ILoginService.LoginResult.Success) return;
            base.OnInitialized();

            #region loading modules

            moduleManager.LoadModule<HomeModule>();
            moduleManager.LoadModule<ContractModule>();

            #endregion loading modules

            #region registering views

            var regionManager = Container.Resolve<IRegionManager>();
            var bottomInfoLine = Container.Resolve<BottomInfoLine>();
            regionManager.Regions[RegionNames.BottomInfoLineRegion].Add(bottomInfoLine);

            #endregion registering views

            #endregion Login
        }
    }
}