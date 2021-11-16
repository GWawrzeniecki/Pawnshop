using PawnShop.Core.Attributes;
using PawnShop.Core.Regions;
using PawnShop.Modules.Sale.Dialogs.ViewModels;
using PawnShop.Modules.Sale.Dialogs.Views;
using PawnShop.Modules.Sale.MenuItem;
using PawnShop.Modules.Sale.Validators;
using PawnShop.Modules.Sale.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PawnShop.Modules.Sale
{
    [Privilege("PawnShopTabs")]
    [Order(4)]
    public class SaleModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public SaleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.MenuRegion].Add(containerProvider.Resolve<SaleHamburgerMenuItem>());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.Sale, SaleViewModel>();
            containerRegistry.Register<SaleValidator>();
            containerRegistry.RegisterDialog<ShowPreview, ShowPreviewViewModel>();
        }
    }
}