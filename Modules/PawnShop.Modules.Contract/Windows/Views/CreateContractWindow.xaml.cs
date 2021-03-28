using MahApps.Metro.Controls;
using Prism.Regions;

namespace PawnShop.Modules.Contract.Windows.Views
{
    /// <summary>
    /// Interaction logic for AddContractWindow.xaml
    /// </summary>
    public partial class CreateContractWindow : MetroWindow
    {
        public CreateContractWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionManager(this.HamburgerMenuItemCollection, regionManager);
            RegionManager.SetRegionManager(this.CreateContractContentControl, regionManager);
        }
    }
}