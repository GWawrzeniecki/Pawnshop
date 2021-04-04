using PawnShop.Business.Models;
using PawnShop.Core;
using PawnShop.Core.Dialogs;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Login.Extensions;
using PawnShop.Modules.Login.Validators;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors.Layout;

namespace PawnShop.Modules.Login.ViewModels
{
    public class LoginDialogViewModel : ViewModelBase<LoginDialogViewModel>, IDialogAware
    {
        #region private members

        private readonly ILoginService _loginService;
        private readonly IDialogService _dialogService;
        private readonly IUIService _uiService;
        private bool _userNameHasText;
        private bool _passwordBoxHasText;
        private bool _passwordTag;
        private string _userName;
        private DelegateCommand<PasswordBox> _loginCommand;
        private bool _loginButtonIsBusy;

        #endregion private members

        #region public members

        public string Title => "Lombard \"VIP\"";

        public event Action<IDialogResult> RequestClose;

        #endregion public members

        #region commands

        public DelegateCommand<PasswordBox> LoginCommand => _loginCommand ??= new DelegateCommand<PasswordBox>(LoginAsync, CanLogin);

        #endregion commands

        #region public properties

        public bool UserNameHasText
        {
            get => _userNameHasText;
            set => SetProperty(ref _userNameHasText, value);
        }

        public bool PasswordBoxHasText
        {
            get => _passwordBoxHasText;
            set => SetProperty(ref _passwordBoxHasText, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public bool PasswordTag
        {
            get => _passwordTag;
            set => SetProperty(ref _passwordTag, value);
        }

        #endregion public properties

        #region constructor

        public LoginDialogViewModel(ILoginService loginService, IDialogService dialogService, IUIService uService, LoginDialogValidator loginDialogValidator) : base(loginDialogValidator)
        {
            LoginCommand.ObservesProperty(() => UserNameHasText).ObservesProperty(() => PasswordBoxHasText);
            this._loginService = loginService;
            this._dialogService = dialogService;
            this._uiService = uService;
            PasswordTag = true;
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
                _loginButtonIsBusy = true;
                AutoLoginAdmin(passwordBox);
                var password = passwordBox.GetReadOnlyCopy();
                _uiService.SetMouseBusyCursor();
                var (success, loggedUser) = await TryToLoginAsync(UserName, password);
                if (success)
                {
                    await TryToStartStartupProcedures(loggedUser);
                    CloseDialogWithSuccess();
                }

                _uiService.ResetMouseCursor();
            }
            catch (LoginException loginException)
            {
                _uiService.ResetMouseCursor();
                _dialogService.ShowNotificationDialog("Błąd",
                    $"{loginException.Message}{Environment.NewLine}Błąd: {loginException.InnerException.Message}",
                    null);
            }
            catch (LoadingStartupDataException loadingStartupDataException)
            {
                _uiService.ResetMouseCursor();
                _dialogService.ShowNotificationDialog("Błąd",
                    $"{loadingStartupDataException.Message}{Environment.NewLine}Błąd: {loadingStartupDataException.InnerException.Message}",
                    null);
            }
            catch (UpdatingContractStatesException updatingContractException)
            {
                _uiService.ResetMouseCursor();
                _dialogService.ShowNotificationDialog("Błąd",
                    $"{updatingContractException.Message}{Environment.NewLine}Błąd: {updatingContractException.InnerException.Message}",
                    null);
            }
            catch (Exception e)
            {
                _uiService.ResetMouseCursor();
                _dialogService.ShowNotificationDialog("Błąd",
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}", null);
            }
            finally
            {
                _loginButtonIsBusy = false;
            }
        }

        private async Task TryToStartStartupProcedures(WorkerBoss loggedUser)
        {
            await _loginService.LoadStartupData(loggedUser);
            await _loginService.UpdateContractStates();
        }

        #endregion command methods

        #region private methods

        private bool CanLogin(PasswordBox passwordBox)
        {
            //return UserNameHasText && PasswordBoxHasText && !_loginButtonIsBusy;
            return true && !_loginButtonIsBusy; // For fast login while developing
        }

        private async Task<(bool, WorkerBoss)> TryToLoginAsync(string userName, SecureString password)
        {
            (bool success, WorkerBoss loggedUser) = await _loginService.LoginAsync(userName, password);
            PasswordTag = success;

            return (success, loggedUser);
        }

        private void CloseDialog(ButtonResult buttonResult) => RequestClose?.Invoke(new DialogResult(buttonResult));

        private void AutoLoginAdmin(PasswordBox passwordBox)
        {
            UserName = "grzegorz.wawrzeniecki";
            passwordBox.Password = "testtesttest";
        }

        private void CloseDialogWithSuccess()
        {
            CloseDialog(ButtonResult.OK);
        }

        #endregion private methods

        #region viewModelBase

        protected override LoginDialogViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase
    }
}