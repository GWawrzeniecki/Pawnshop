using System.Windows.Controls;
using PawnShop.Core.Taskbar;
using PawnShop.Modules.Contract.Taskbar.Views;

namespace PawnShop.Modules.Contract.Views
{
    /// <summary>
    ///     Interaction logic for Contract
    /// </summary>
    [DependentView(typeof(ContractTaskbar), "TopTaskBarRegion")]
    public partial class Contract : UserControl, ISupportDataContext
    {
        public Contract()
        {
            InitializeComponent();
        }
    }
}