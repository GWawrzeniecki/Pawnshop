using MahApps.Metro.Controls;
using PawnShop.Core;
using PawnShop.Core.Regions;
using PawnShop.Modules.Login.Dialogs;
using PawnShop.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Windows;
using System.Windows.Controls;
using PawnShop.Core.Dialogs;

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
            var dialogService = Container.Resolve<IDialogService>();

            dialogService.ShowLogInDialog(c =>
            {
                if (c.Result == ButtonResult.OK)
                    base.OnInitialized();
                else
                    Application.Current.Shutdown();
            });

            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.TopTaskBarRegion, typeof(BaseTaskBar));



        }
    }
}
