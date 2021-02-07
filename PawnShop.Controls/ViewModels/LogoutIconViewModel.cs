using PawnShop.Core.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Windows;

namespace PawnShop.CoreViews.ViewModels
{
    public class LogoutIconViewModel : BindableBase
    {
        #region private members

        private DelegateCommand<string> _logoutCommand;
        private readonly IDialogService _dialogService;

        #endregion private members

        #region public properties

        public DelegateCommand<string> LogoutCommand =>
_logoutCommand ??= new DelegateCommand<string>(Logout);

        #endregion public properties

        #region constructor

        public LogoutIconViewModel(IDialogService dialogService)
        {
            this._dialogService = dialogService;
        }

        #endregion constructor

        #region command methods

        private void Logout(string obj)
        {
            Application.Current.MainWindow.Hide();

            _dialogService.ShowLogInDialog(c =>
            {
                if (c.Result == ButtonResult.OK)
                    Application.Current.MainWindow.Show();
                else
                    Application.Current.Shutdown(1);
            });
        }

        #endregion command methods
    }
}