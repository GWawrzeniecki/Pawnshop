using PawnShop.Controls.BaseTaskbar.Views;
using PawnShop.Core.Taskbar;
using System.Windows.Controls;

namespace PawnShop.Modules.Settings.Views
{
    /// <summary>
    /// Interaction logic for Settings
    /// </summary>
    [DependentView(typeof(BaseTaskBar), "TopTaskBarRegion")]
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }
    }
}
