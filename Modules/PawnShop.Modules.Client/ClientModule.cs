using PawnShop.Core.Regions;
using PawnShop.Modules.Client.MenuItem;
using PawnShop.Modules.Client.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PawnShop.Modules.Client
{
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
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.Client, ClientViewModel>();
        }
    }
}