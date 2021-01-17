using PawnShop.Services;
using PawnShop.Services.DataService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PawnShop.Modules.Login.Dialogs
{
    public class LoginDialogViewModel : BindableBase, IDialogAware
    {

        #region private members
        private DelegateCommand<PasswordBox> _loginCommand;
        private readonly IHashService _hashService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion

        #region public members
        public string Title => "Logowanie";
        public event Action<IDialogResult> RequestClose;
        #endregion

        #region public properties
        public DelegateCommand<PasswordBox> LoginCommand => _loginCommand ??= new DelegateCommand<PasswordBox>(Login);
        #endregion

        #region constructor
        public LoginDialogViewModel(IHashService hashService, IUnitOfWork unitOfWork)
        {
            this._hashService = hashService;
            this._unitOfWork = unitOfWork;
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

        #region command methods
        private void Login(PasswordBox passwordBox) // wiem, ze to psuje pattern MVVM ale ze wzgledow bezpieczenstwa nie robie Dependency property, to do check Mahapps PassswordBox implementation
        {

            var hash = _hashService.Hash(passwordBox.Password);



            var test = _hashService.Check(hash, passwordBox.Password);

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }
        #endregion
    }
}
