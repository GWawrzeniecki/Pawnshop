using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows.Controls;

namespace PawnShop.Modules.Login.Dialogs
{
    public class LoginDialogViewModel : BindableBase, IDialogAware
    {
        #region private members

        private bool _userNameHasText;
        private bool _passwordBoxHasText;
        private DelegateCommand<PasswordBox> _loginCommand;
        private readonly IHashService _hashService;
        private readonly IUnitOfWork _unitOfWork;

        #endregion private members

        #region public members

        public string Title => "Logowanie";

        public event Action<IDialogResult> RequestClose;

        #endregion public members

        #region public properties

        public DelegateCommand<PasswordBox> LoginCommand => _loginCommand ??= new DelegateCommand<PasswordBox>(Login, CanLogin);

        public bool UserNameHasText
        {
            get { return _userNameHasText; }
            set { SetProperty(ref _userNameHasText, value); }
        }

        public bool PasswordBoxHasText
        {
            get { return _passwordBoxHasText; }
            set { SetProperty(ref _passwordBoxHasText, value); }
        }

        #endregion public properties

        #region constructor

        public LoginDialogViewModel(IHashService hashService, IUnitOfWork unitOfWork)
        {
            this._hashService = hashService;
            this._unitOfWork = unitOfWork;
            LoginCommand.ObservesProperty(() => UserNameHasText).ObservesProperty(() => PasswordBoxHasText);
        }

        #endregion constructor

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

        #endregion iDialogAware

        #region command methods

        private bool CanLogin(PasswordBox arg)
        {
            return UserNameHasText && PasswordBoxHasText;
        }

        private void Login(PasswordBox passwordBox) // wiem, ze to psuje pattern MVVM ale ze wzgledow bezpieczenstwa nie robie Dependency property, to do check Mahapps PassswordBox implementation
        {
            try
            {
                TryToLogin(passwordBox);
            }
            catch (Exception e)
            {
            }
        }

        private void TryToLogin(PasswordBox passwordBox)
        {
            var hash = _hashService.Hash(passwordBox.Password);
            var test = _hashService.Check(hash, passwordBox.Password);

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        #endregion command methods
    }
}