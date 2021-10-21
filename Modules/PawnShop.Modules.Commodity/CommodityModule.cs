using PawnShop.Core.Attributes;
using PawnShop.Core.Regions;
using PawnShop.Modules.Commodity.MenuItem;
using PawnShop.Modules.Commodity.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PawnShop.Modules.Commodity
{
    [Privilege("PawnShopTabs")]
    [Order(3)]
    public class CommodityModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public CommodityModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.MenuRegion].Add(containerProvider.Resolve<CommodityHamburgerMenuItem>());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.Commodity, CommodityViewModel>();
            containerRegistry.Register<CommodityHamburgerMenuItem>();
        }
    }
}