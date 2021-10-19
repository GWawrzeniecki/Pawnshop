using PawnShop.Core.Taskbar;
using PawnShop.Modules.Sale.TaskBar;
using System.Windows.Controls;

namespace PawnShop.Modules.Sale.Views
{
    /// <summary>
    /// Interaction logic for Sale
    /// </summary>
    [DependentView(typeof(SaleTaskBar), "TopTaskBarRegion")]
    public partial class Sale : UserControl, ISupportDataContext
    {
        public Sale()
        {
            InitializeComponent();
        }
    }
}
