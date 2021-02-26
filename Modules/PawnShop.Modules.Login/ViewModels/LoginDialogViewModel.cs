using PawnShop.Core.Dialogs;
using PawnShop.Modules.Login.Extensions;
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
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PawnShop.Modules.Login.ViewModels
{
    public class LoginDialogViewModel : BindableBase, IDialogAware, INotifyDataErrorInfo
    {
        #region private members

        protected readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        private readonly ILoginService _loginService;
        private readonly IDialogService _dialogService;
        private readonly IUIService _uiService;
        private bool _userNameHasText;
        private bool _passwordBoxHasText;
        private bool _passwordTag;
        private string _userName;
        private DelegateCommand<PasswordBox> _loginCommand;

        #endregion private members

        #region public members

        public string Title => "Logowanie";
        public bool HasErrors => _errorsByPropertyName.Any();

        public event Action<IDialogResult> RequestClose;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion public members

        #region public properties

        public DelegateCommand<PasswordBox> LoginCommand => _loginCommand ??= new DelegateCommand<PasswordBox>(LoginAsync, CanLogin);

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

        public LoginDialogViewModel(ILoginService loginService, IDialogService dialogService, IUIService uService)
        {
            LoginCommand.ObservesProperty(() => UserNameHasText).ObservesProperty(() => PasswordBoxHasText);
            this._loginService = loginService;
            this._dialogService = dialogService;
            this._uiService = uService;
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

        private async void LoginAsync(PasswordBox passwordBox)
        {
            try
            {
                var password = passwordBox.GetReadOnlyCopy();
                _uiService.SetMouseBusyCursor();
                await TryToLoginAsync(UserName, password);
                _uiService.ResetMouseCursor();
            }
            catch (Exception e)
            {
                _uiService.ResetMouseCursor();
                _dialogService.ShowNotificationDialog("Błąd logowania", $"Wystąpił błąd podczas logowania.{Environment.NewLine}Błąd: {e.Message}", null);
            }

        }

        #endregion command methods

        #region private methods

        private bool CanLogin(PasswordBox passwordBox)
        {
            return UserNameHasText && PasswordBoxHasText;
        }

        private async Task TryToLoginAsync(string userName, SecureString password)
        {
            var isValidPassword = await _loginService.LoginAsync(userName, password);

            if (!isValidPassword)
            {
                AddError(nameof(PasswordTag), "Login lub hasło jest nieprawidłowe.");
            }
            else
            {
                ClearError(nameof(PasswordTag), "Login lub hasło jest nieprawidłowe.");
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
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