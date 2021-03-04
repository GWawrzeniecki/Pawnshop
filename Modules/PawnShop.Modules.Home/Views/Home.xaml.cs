using PawnShop.Core.Taskbar;
using PawnShop.Controls.BaseTaskbar.Views;
using System.Windows.Controls;

namespace PawnShop.Modules.Home.Views
{
    /// <summary>
    /// Interaction logic for Home
    /// </summary>
    [UserControl(typeof(BaseTaskBar))]
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }
    }
}