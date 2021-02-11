using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Windows.Controls;

namespace PawnShop.Modules.Login.Dialogs
{
    public class LoginDialogViewModel : BindableBase, IDialogAware, INotifyDataErrorInfo
    {
        #region private members

        protected readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        private bool _userNameHasText;
        private bool _passwordBoxHasText;
        private bool _passwordTag;
        private string _userName;
        private DelegateCommand<PasswordBox> _loginCommand;
        private readonly IHashService _hashService;
        private readonly IUnitOfWork _unitOfWork;

        #endregion private members

        #region public members

        public string Title => "Logowanie";
        public bool HasErrors => _errorsByPropertyName.Any();

        public event Action<IDialogResult> RequestClose;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public bool PasswordTag
        {
            get { return _passwordTag; }
            set { SetProperty(ref _passwordTag, value); }
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

        private void Login(PasswordBox passwordBox)
        {
            try
            {
                var pwd = passwordBox.SecurePassword.Copy();
                pwd.MakeReadOnly();
                TryToLogin(UserName, pwd);
            }
            catch (Exception e)
            {
            }
            finally
            {
            }
        }

        #endregion command methods

        #region private methods

        private bool CanLogin(PasswordBox passwordBox)
        {
            return UserNameHasText && PasswordBoxHasText;
        }

        private void TryToLogin(string userName, SecureString password)
        {
            var passwordHash = _hashService.Hash(password);
            var isValidPassword = _hashService.Check(passwordHash, password);

            isValidPassword = false;

            if (!isValidPassword)
                AddError(nameof(PasswordTag), "Login lub hasło jest nieprawidłowe.");
            else
                ClearError(nameof(PasswordTag), "Login lub hasło jest nieprawidłowe.");

            //RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        #endregion private methods

        #region INotifyDataError

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorsByPropertyName.ContainsKey(propertyName) ?
            _errorsByPropertyName[propertyName] : null;
        }

        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearError(string propertyName, string error)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                var list = _errorsByPropertyName[propertyName];
                list.Remove(error);

                if (list.Count == 0)
                {
                    _errorsByPropertyName.Remove(propertyName);
                }

                OnErrorsChanged(propertyName);
            }
        }

        #endregion INotifyDataError
    }
}