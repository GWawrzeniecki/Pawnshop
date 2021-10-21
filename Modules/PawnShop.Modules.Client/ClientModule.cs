using PawnShop.Core.Attributes;
using PawnShop.Core.Regions;
using PawnShop.Modules.Client.MenuItem;
using PawnShop.Modules.Client.Validators;
using PawnShop.Modules.Client.ViewModels;
using PawnShop.Modules.Client.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PawnShop.Modules.Client
{
    [Privilege("PawnShopTabs")]
    [Order(5)]
    public class ClientModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ClientModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.MenuRegion].Add(containerProvider.Resolve<ClientHamburgerMenuItem>());
            _regionManager.RegisterViewWithRegion<DealTab>(RegionNames.ClientTabControlRegion);
            _regionManager.RegisterViewWithRegion<DetailTab>(RegionNames.ClientTabControlRegion);

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.Client, ClientViewModel>();
            containerRegistry.RegisterForNavigation<DealTab, DealTabViewModel>();
            containerRegistry.RegisterForNavigation<DetailTab, DetailTabViewModel>();
            containerRegistry.Register<ClientViewModelValidator>();

        }
    }
}