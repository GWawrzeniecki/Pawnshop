using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace PawnShop.CoreViews.ViewModels
{
    public class LogoutIconViewModel : BindableBase
    {
        #region private members

        private DelegateCommand _logoutCommand;

        private readonly ILoginService _loginService;

        #endregion private members

        #region public properties

        public DelegateCommand LogoutCommand =>
_logoutCommand ??= new DelegateCommand(Logout);

        #endregion public properties

        #region constructor

        public LogoutIconViewModel(ILoginService loginService)
        {
            this._loginService = loginService;
        }

        #endregion constructor

        #region command methods

        private void Logout()
        {
            _loginService.ShowLogoutDialog();
        }

        #endregion command methods
    }
}