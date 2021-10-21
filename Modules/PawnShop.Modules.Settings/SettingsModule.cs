using PawnShop.Core.Attributes;
using PawnShop.Core.Regions;
using PawnShop.Modules.Settings.Dialogs.ViewModels;
using PawnShop.Modules.Settings.Dialogs.Views;
using PawnShop.Modules.Settings.MenuItem;
using PawnShop.Modules.Settings.Validators;
using PawnShop.Modules.Settings.ViewModels;
using PawnShop.Modules.Settings.Views;
using PawnShop.Services.Implementations;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PawnShop.Modules.Settings
{
    [Privilege("SettingsTab")]
    [Order(7)]
    public class SettingsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public SettingsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.MenuRegion].Add(containerProvider.Resolve<SettingsHamburgerMenuItem>());
            _regionManager.RegisterViewWithRegion<AppSettings>(RegionNames.SettingsTabControlRegion);
            _regionManager.RegisterViewWithRegion<PawnShopSettings>(RegionNames.SettingsTabControlRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.Settings, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<AppSettings, AppSettingsViewModel>();
            containerRegistry.RegisterForNavigation<PawnShopSettings, PawnShopSettingsViewModel>();
            containerRegistry.RegisterDialog<LendingRateSettingsDialog, LendingRateSettingsDialogViewModel>();
            containerRegistry.RegisterDialog<EditLendingRateDialog, EditLendingRateDialogViewModel>();
            containerRegistry.Register<ILendingRateService, LendingRateService>();
            containerRegistry.Register<LendingRateSettingsDialogValidator>();
            containerRegistry.Register<EditLendingRateDialogValidator>();
            containerRegistry.Register<PawnShopSettingsValidator>();
        }
    }
}