using MahApps.Metro.Controls;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Core.Taskbar;
using PawnShop.Modules.Contract.MenuItem;
using Prism.Ioc;
using Prism.Regions;

namespace PawnShop.Modules.Contract.Windows.Views
{
    /// <summary>
    /// Interaction logic for BuyBackContractWindow.xaml
    /// </summary>
    public partial class BuyBackContractWindow : MetroWindow, IScopedWindow, ISupportDataContext
    {
        public BuyBackContractWindow(IContainerProvider containerProvider, IRegionManager scopedRegionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionManager(HamburgerMenuItemCollection, scopedRegionManager);
            RegionManager.SetRegionManager(CreateContractContentControl, scopedRegionManager);
            RegisterViews(containerProvider, scopedRegionManager);
            DisabledHamburgerMenuItemsOnStart(containerProvider);
        }

        public void RegisterViews(IContainerProvider containerProvider, IRegionManager scopedRegionManager)
        {
            var buyBackContractDataHamburgerMenuItem = containerProvider.Resolve<BuyBackContractDataHamburgerMenuItem>();
            var buyBackContractItemsHamburgerMenuItem = containerProvider.Resolve<BuyBackContractItemsHamburgerMenuItem>();
            var buyBackContactPaymentHamburgerMenuItem = containerProvider.Resolve<BuyBackContactPaymentHamburgerMenuItem>();

            RegionManagerAware.SetRegionManagerAware(buyBackContractDataHamburgerMenuItem,
                scopedRegionManager); // adding here because hmi doesn't have view model
            RegionManagerAware.SetRegionManagerAware(buyBackContractItemsHamburgerMenuItem, scopedRegionManager);
            RegionManagerAware.SetRegionManagerAware(buyBackContactPaymentHamburgerMenuItem, scopedRegionManager);

            scopedRegionManager.Regions[RegionNames.MenuRegion].Add(buyBackContractDataHamburgerMenuItem);
            scopedRegionManager.Regions[RegionNames.MenuRegion].Add(buyBackContractItemsHamburgerMenuItem);
            scopedRegionManager.Regions[RegionNames.MenuRegion].Add(buyBackContactPaymentHamburgerMenuItem);

        }

        private void DisabledHamburgerMenuItemsOnStart(IContainerProvider containerProvider)
        {
            //var renewContractPaymentHamburgerMenuItem = containerProvider.Resolve<RenewContractPaymentHamburgerMenuItem>();
            //renewContractPaymentHamburgerMenuItem.IsEnabled = false;
        }
    }
}
