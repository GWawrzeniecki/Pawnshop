using PawnShop.Core.ScopedRegion;
using PawnShop.Modules.Sale.Base;
using Prism.Regions;
using System.Windows;

namespace PawnShop.Modules.Sale.Views
{
    /// <summary>
    /// Interaction logic for SaleAdditionalInfo
    /// </summary>
    public partial class SaleAdditionalInfo : SaleInfoViewBase
    {
        public SaleAdditionalInfo(IRegionManager regionManager)
        {
            InitializeComponent();
            var scopedRegionManager = regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(AdditionalInfoContentControl, scopedRegionManager);
            RegionManager.SetRegionManager(SaleInfoContentControl, scopedRegionManager);
            RegionManagerAware.SetRegionManagerAware(this, scopedRegionManager);
            SaleInfoContentControl.Loaded += SaleInfoContentControl_Loaded;
        }

        private void SaleInfoContentControl_Loaded(object sender, RoutedEventArgs e)
        {
            AdjustGrids(AdditionalInfoContentControl, SaleInfoContentControl, SellGroupBox);
        }

    }
}
