using PawnShop.Core.Regions;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Contract.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PawnShop.Modules.Contract
{
    public class ContractModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ContractModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MenuRegion, typeof(ContractHamburgerMenuItem));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.Contract, ContractViewModel>();
        }
    }
}