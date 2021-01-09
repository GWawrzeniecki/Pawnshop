using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PawnShop.Modules.Login.Dialogs
{
    public class LoginDialogViewModel : BindableBase, IDialogAware
    {

        #region private members
        private DelegateCommand _loginCommand;
        #endregion

        #region public members
        public string Title => "Logowanie";
        public event Action<IDialogResult> RequestClose;
        #endregion

        #region public properties
        public DelegateCommand LoginCommand => _loginCommand ??= new DelegateCommand(LogIn);
        #endregion

        #region constructor
        public LoginDialogViewModel()
        {
            
        }


        #endregion


        #region iDialogAware
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }

        #endregion

        #region private methods
        private void LogIn()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
        #endregion
    }
}
