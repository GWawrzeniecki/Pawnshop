using PawnShop.Core.Regions;
using PawnShop.Modules.Settings.MenuItem;
using PawnShop.Modules.Settings.ViewModels;
using PawnShop.Modules.Settings.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PawnShop.Modules.Settings
{
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
            _regionManager.RegisterViewWithRegion<AppSettings>(RegionNames.TabControlRegion);
            _regionManager.RegisterViewWithRegion<PawnShopSettings>(RegionNames.TabControlRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.Settings, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<Views.AppSettings, AppSettingsViewModel>();
            containerRegistry.RegisterForNavigation<Views.PawnShopSettings, PawnShopSettingsViewModel>();
        }
    }
}