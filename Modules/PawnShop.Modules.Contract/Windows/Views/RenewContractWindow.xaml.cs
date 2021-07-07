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
    /// Interaction logic for RenewContractWindow.xaml
    /// </summary>
    public partial class RenewContractWindow : MetroWindow, IScopedWindow, ISupportDataContext
    {
        public RenewContractWindow(IContainerProvider containerProvider, IRegionManager scopedRegionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionManager(HamburgerMenuItemCollection, scopedRegionManager);
            RegionManager.SetRegionManager(CreateContractContentControl, scopedRegionManager);
            RegisterViews(containerProvider, scopedRegionManager);
            DisabledHamburgerMenuItemsOnStart(containerProvider);

        }



        public void RegisterViews(IContainerProvider containerProvider, IRegionManager scopedRegionManager)
        {
            var renewContractDataHamburgerMenuItem = containerProvider.Resolve<RenewContractDataHamburgerMenuItem>();
            var renewContractPaymentHamburgerMenuItem = containerProvider.Resolve<RenewContractPaymentHamburgerMenuItem>();
            RegionManagerAware.SetRegionManagerAware(renewContractPaymentHamburgerMenuItem,
                scopedRegionManager); // adding here because hmi doesn't have view model
            RegionManagerAware.SetRegionManagerAware(renewContractDataHamburgerMenuItem, scopedRegionManager);
            scopedRegionManager.Regions[RegionNames.MenuRegion].Add(renewContractDataHamburgerMenuItem);
            scopedRegionManager.Regions[RegionNames.MenuRegion].Add(renewContractPaymentHamburgerMenuItem);
        }

        private void DisabledHamburgerMenuItemsOnStart(IContainerProvider containerProvider)
        {
            var renewContractPaymentHamburgerMenuItem = containerProvider.Resolve<RenewContractPaymentHamburgerMenuItem>();
            renewContractPaymentHamburgerMenuItem.IsEnabled = false;
        }
    }
}
