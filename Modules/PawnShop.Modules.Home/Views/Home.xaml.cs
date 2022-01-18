using PawnShop.Controls.BaseTaskbar.Views;
using PawnShop.Core.Taskbar;
using System.Windows.Controls;

namespace PawnShop.Modules.Home.Views
{
    /// <summary>
    /// Interaction logic for Home
    /// </summary>
    [DependentView(typeof(BaseTaskBar), "TopTaskBarRegion")]
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }


    }
}