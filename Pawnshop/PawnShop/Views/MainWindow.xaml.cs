using System.Windows;
using MahApps.Metro.Controls;

using PawnShop.Core.Dialogs;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace PawnShop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly IDialogService _dialogService;

        public MainWindow(IRegionManager regionManager, IDialogService dialogService)
        {
            _dialogService = dialogService;
            InitializeComponent();
            RegionManager.SetRegionManager(this.HamburgerMenuItemCollection, regionManager);
            RegionManager.SetRegionManager(this.ContentRegionControl, regionManager);
            
        }

       
    }
}