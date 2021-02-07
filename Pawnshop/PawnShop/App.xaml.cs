using MahApps.Metro.Controls;
using PawnShop.Core;
using PawnShop.Core.Dialogs;
using PawnShop.Core.Regions;
using PawnShop.Modules.Login.Dialogs;
using PawnShop.Services.DataService;
using PawnShop.Services.Implementations;
using PawnShop.Services.Interfaces;
using PawnShop.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;
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
            containerRegistry.RegisterSingleton<ISecretManagerService, SecretManagerService>();
            containerRegistry.RegisterSingleton<IHashService, HashService>();
            containerRegistry.RegisterSingleton<IAesService, AesService>();
            containerRegistry.RegisterSingleton<IUnitOfWork, UnitOfWork>();
            containerRegistry.RegisterDialog<LoginDialog, LoginDialogViewModel>();
            containerRegistry.RegisterDialogWindow<MahappsDialogWindow>();
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
        }

        protected override void OnInitialized()
        {
            #region Login

            var dialogService = Container.Resolve<IDialogService>();

            dialogService.ShowLogInDialog(c =>
            {
                if (c.Result == ButtonResult.OK)
                    base.OnInitialized();
                else
                    Application.Current.Shutdown();
            });

            #endregion Login

            #region registering views

            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.TopTaskBarRegion, typeof(BaseTaskBar));

            #endregion registering views
        }
    }
}