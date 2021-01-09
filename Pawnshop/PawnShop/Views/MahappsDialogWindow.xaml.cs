using MahApps.Metro.Controls;
using Prism.Services.Dialogs;
using System.Windows;

namespace PawnShop.Views
{
    /// <summary>
    /// Interaction logic for MahappsDialogWindow.xaml
    /// </summary>
    public partial class MahappsDialogWindow : MetroWindow, IDialogWindow
    {
        public MahappsDialogWindow()
        {
            InitializeComponent();
        }

        public IDialogResult Result { get; set; }
    }

}
