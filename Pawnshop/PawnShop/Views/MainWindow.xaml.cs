using MahApps.Metro.Controls;
using PawnShop.Core.Regions;
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
            RegionManager.SetRegionName(this.contentRegionControl, RegionNames.ContentRegion); //lazily created control 
            RegionManager.SetRegionManager(this.contentRegionControl, regionManager);
        }
    }
}