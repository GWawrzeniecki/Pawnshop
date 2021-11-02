using Prism.Common;
using System.Windows.Controls;

namespace PawnShop.Modules.Commodity.Views
{
    /// <summary>
    /// Interaction logic for Sale
    /// </summary>
    public partial class PutOnSale : UserControl
    {
        public PutOnSale()
        {
            InitializeComponent();
            this.Loaded += PutOnSale_Loaded;
        }

        private void PutOnSale_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ObservableObject<object> viewRegionContext = Prism.Regions.RegionContext.GetObservableContext(this);
            viewRegionContext.Value = Grid.ColumnDefinitions[0].ActualWidth;
        }
    }
}
