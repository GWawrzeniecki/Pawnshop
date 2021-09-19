using PawnShop.Core.Taskbar;
using PawnShop.Modules.Worker.TaskBar;
using System.Windows.Controls;

namespace PawnShop.Modules.Worker.Views
{
    /// <summary>
    /// Interaction logic for Workers
    /// </summary>
    [DependentView(typeof(WorkerTaskBar), "TopTaskBarRegion")]
    public partial class Workers : UserControl, ISupportDataContext
    {
        public Workers()
        {
            InitializeComponent();
        }
    }
}
