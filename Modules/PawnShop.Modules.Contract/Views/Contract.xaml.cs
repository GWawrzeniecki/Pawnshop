using PawnShop.Core.Taskbar;
using PawnShop.Modules.Contract.Taskbar.Views;
using System.Windows.Controls;

namespace PawnShop.Modules.Contract.Views
{
    /// <summary>
    /// Interaction logic for Contract
    /// </summary>
    [UserControl(typeof(ContractTaskbar))]
    public partial class Contract : UserControl
    {
        public Contract()
        {
            InitializeComponent();
        }
    }
}