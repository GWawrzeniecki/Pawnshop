using PawnShop.Core.Taskbar;
using PawnShop.Modules.Client.TaskBar;
using System.Windows.Controls;

namespace PawnShop.Modules.Client.Views
{
    /// <summary>
    /// Interaction logic for Client
    /// </summary>
    [DependentView(typeof(ClientTaskBar), "TopTaskBarRegion")]
    public partial class Client : UserControl, ISupportDataContext
    {
        public Client()
        {
            InitializeComponent();
        }
    }
}
