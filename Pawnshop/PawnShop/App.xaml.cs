using MahApps.Metro.Controls;
using PawnShop.Core;
using PawnShop.Core.Regions;
using PawnShop.Core.SharedVariables;
using PawnShop.Dialogs.Views;
using PawnShop.Dialogs.ViewsModels;
using PawnShop.Modules.Home;
using PawnShop.Modules.Home.Views;
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
            containerRegistry.RegisterSingleton<IApllicationCommands, ApplicationCommands>();
            containerRegistry.RegisterSingleton<IUnitOfWork, UnitOfWork>();
            containerRegistry.RegisterSingleton<IUIService, UIService>();
            containerRegistry.RegisterSingleton<ISessionContext, SessionContext>();
            containerRegistry.RegisterDialogWindow<MahappsDialogWindow>();
            containerRegistry.RegisterDialog<LoginDialog, LoginDialogViewModel>();
            containerRegistry.RegisterDialog<NotificationDialog, NotificationDialogViewModel>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            regionAdapterMappings.RegisterMapping<StackPanel>(Container.Resolve<StackPanelRegionAdapter>());
            regionAdapterMappings.RegisterMapping<HamburgerMenuItemCollection>(Container.Resolve<HamburgerMenuItemCollectionRegionAdapter>());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<LoginModule>();
            moduleCatalog.AddModule<HomeModule>();
        }

        protected override void OnInitialized()
        {
            #region Login

            var loginService = Container.Resolve<ILoginService>();
            var result = loginService.ShowLoginDialog();

            if (result == ILoginService.LoginResult.Success)
            {
                base.OnInitialized();

                #region registering views

                var regionManager = Container.Resolve<IRegionManager>();
                regionManager.RegisterViewWithRegion(RegionNames.TopTaskBarRegion, typeof(BaseTaskBar));
                regionManager.RegisterViewWithRegion(RegionNames.BottomInfoLineRegion, typeof(BottomInfoLine));
                regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Home));

                #endregion registering views
            }

            #endregion Login
        }
    }
}