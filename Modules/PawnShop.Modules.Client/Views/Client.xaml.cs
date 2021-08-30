using PawnShop.Controls.BaseTaskbar.Views;
using PawnShop.Core.Taskbar;
using System.Windows.Controls;

namespace PawnShop.Modules.Client.Views
{
    /// <summary>
    /// Interaction logic for Client
    /// </summary>
    [DependentView(typeof(BaseTaskBar), "TopTaskBarRegion")]
    public partial class Client : UserControl
    {
        public Client()
        {
            InitializeComponent();
        }
    }
}
