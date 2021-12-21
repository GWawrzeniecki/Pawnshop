using MahApps.Metro.Controls;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Core.Taskbar;
using PawnShop.Modules.Contract.MenuItem;
using Prism.Ioc;
using Prism.Regions;

namespace PawnShop.Modules.Contract.Windows.Views
{

    public partial class CreateContractWindow : MetroWindow, IScopedWindow, ISupportDataContext
    {
        public CreateContractWindow(IContainerProvider containerProvider, IRegionManager scopedRegionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionManager(HamburgerMenuItemCollection, scopedRegionManager);
            RegionManager.SetRegionManager(CreateContractContentControl, scopedRegionManager);
            RegisterViews(containerProvider, scopedRegionManager);
            DisabledHamburgerMenuItemsOnStart(containerProvider);
        }



        public void RegisterViews(IContainerProvider containerProvider, IRegionManager scopedRegionManager)
        {
            var clientDataHamburgerMenuItem = containerProvider.Resolve<CreateContractClientDataHamburgerMenuItem>();
            var contractDataHamburgerMenuItem = containerProvider.Resolve<CreateContractContractDataHamburgerMenuItem>();
            var summaryHamburgerMenuItem = containerProvider.Resolve<CreateContractSummaryHamburgerMenuItem>();

            RegionManagerAware.SetRegionManagerAware(clientDataHamburgerMenuItem,
                scopedRegionManager); // adding here because hmi doesn't have view model
            RegionManagerAware.SetRegionManagerAware(contractDataHamburgerMenuItem, scopedRegionManager);
            RegionManagerAware.SetRegionManagerAware(summaryHamburgerMenuItem, scopedRegionManager);
            scopedRegionManager.Regions[RegionNames.MenuRegion].Add(clientDataHamburgerMenuItem);
            scopedRegionManager.Regions[RegionNames.MenuRegion].Add(contractDataHamburgerMenuItem);
            scopedRegionManager.Regions[RegionNames.MenuRegion].Add(summaryHamburgerMenuItem);


        }

        private void DisabledHamburgerMenuItemsOnStart(IContainerProvider containerProvider)
        {
            var contractDataHamburgerMenuItem = containerProvider.Resolve<CreateContractContractDataHamburgerMenuItem>();
            var summaryHamburgerMenuItem = containerProvider.Resolve<CreateContractSummaryHamburgerMenuItem>();
            contractDataHamburgerMenuItem.IsEnabled = false;
            summaryHamburgerMenuItem.IsEnabled = false;
        }


    }
}