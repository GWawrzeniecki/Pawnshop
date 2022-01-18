using MahApps.Metro.Controls;
using Prism.Regions;

namespace PawnShop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();

            RegionManager.SetRegionManager(HamburgerMenuItemCollection, regionManager); // https://stackoverflow.com/questions/53968851/i-am-unable-to-make-a-contentcontrol-a-region-using-prism
            RegionManager.SetRegionManager(ContentRegionControl, regionManager);
        }

    }
}