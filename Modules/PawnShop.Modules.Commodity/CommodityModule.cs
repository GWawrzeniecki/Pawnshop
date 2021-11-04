using PawnShop.Core.Attributes;
using PawnShop.Core.Regions;
using PawnShop.Modules.Commodity.Dialogs.ViewModels;
using PawnShop.Modules.Commodity.Dialogs.Views;
using PawnShop.Modules.Commodity.MenuItem;
using PawnShop.Modules.Commodity.Validators;
using PawnShop.Modules.Commodity.ViewModels;
using PawnShop.Modules.Commodity.Views;
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
            _regionManager.RegisterViewWithRegion<CurrentGoodsGrid>(RegionNames.CommodityTabControlRegion);
            _regionManager.RegisterViewWithRegion<GoodsForSaleGrid>(RegionNames.CommodityTabControlRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.Commodity, CommodityViewModel>();
            containerRegistry.RegisterForNavigation<CurrentGoodsGrid, CurrentGoodsGridViewModel>();
            containerRegistry.RegisterForNavigation<GoodsForSaleGrid, GoodsForSaleGridViewModel>();
            containerRegistry.RegisterForNavigation<PutOnSale, PutOnSaleViewModel>();
            containerRegistry.Register<CommodityHamburgerMenuItem>();
            containerRegistry.Register<PutOnSaleValidator>();
            containerRegistry.RegisterDialog<PreviewPutOnSaleDialog, PreviewPutOnSaleDialogViewModel>();

        }
    }
}