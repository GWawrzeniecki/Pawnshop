using PawnShop.Core.Taskbar;
using PawnShop.Modules.Commodity.TaskBar;
using System.Windows.Controls;

namespace PawnShop.Modules.Commodity.Views
{
    /// <summary>
    /// Interaction logic for Commodity
    /// </summary>
    [DependentView(typeof(CommodityTaskBar), "TopTaskBarRegion")]
    public partial class Commodity : UserControl, ISupportDataContext
    {
        public Commodity()
        {
            InitializeComponent();
        }
    }
}
