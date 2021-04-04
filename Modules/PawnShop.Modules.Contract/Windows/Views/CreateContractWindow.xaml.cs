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
    ///     Interaction logic for AddContractWindow.xaml
    /// </summary>
    public partial class CreateContractWindow : MetroWindow, IScopedWindow, ISupportDataContext
    {
        public CreateContractWindow(IContainerProvider containerProvider, IRegionManager scopedRegionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionManager(HamburgerMenuItemCollection, scopedRegionManager);
            RegionManager.SetRegionManager(CreateContractContentControl, scopedRegionManager);
            RegisterViews(containerProvider, scopedRegionManager);
        }

        public void RegisterViews(IContainerProvider containerProvider, IRegionManager scopedRegionManager)
        {
            var clientDataHamburgerMenuItem = containerProvider.Resolve<ClientDataHamburgerMenuItem>();
            var contractDataHamburgerMenuItem = containerProvider.Resolve<ContractDataHamburgerMenuItem>();
            RegionManagerAware.SetRegionManagerAware(clientDataHamburgerMenuItem,
                scopedRegionManager); // adding here because hmi doesnt have view model
            RegionManagerAware.SetRegionManagerAware(contractDataHamburgerMenuItem, scopedRegionManager);
            scopedRegionManager.Regions[RegionNames.MenuRegion].Add(clientDataHamburgerMenuItem);
            scopedRegionManager.Regions[RegionNames.MenuRegion].Add(contractDataHamburgerMenuItem);
        }
    }
}