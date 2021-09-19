using MahApps.Metro.Controls;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace PawnShop.Views
{
    /// <summary>
    /// Interaction logic for MahappsDialogWindow.xaml
    /// </summary>
    public partial class MahappsDialogWindow : MetroWindow, IDialogWindow
    {
        public MahappsDialogWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            RegionManager.SetRegionManager(this, regionManager); // https://stackoverflow.com/questions/1014948/wpf-prism-v2-region-in-a-modal-dialog-add-region-in-code-behind/1019029#1019029
        }

        public IDialogResult Result { get; set; }
    }
}