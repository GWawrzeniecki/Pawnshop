using System.Windows.Controls;
using PawnShop.Core.Taskbar;
using PawnShop.Modules.Commodity.TaskBar;

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
