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
using BespokeFusion;
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

        public DelegateCommand<PasswordBox> LoginCommand =>
            _loginCommand ??= new DelegateCommand<PasswordBox>(LoginAsync, CanLogin);

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

        public bool LoginButtonIsBusy
        {
            get => _loginButtonIsBusy;
            set => SetProperty(ref _loginButtonIsBusy, value);
        }

        #endregion public properties

        #region constructor

        public LoginDialogViewModel(ILoginService loginService, IDialogService dialogService, IUIService uService,
            LoginDialogValidator loginDialogValidator) : base(loginDialogValidator)
        {
            LoginCommand.ObservesProperty(() => UserNameHasText).ObservesProperty(() => PasswordBoxHasText)
                .ObservesProperty(() => LoginButtonIsBusy);
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
                LoginButtonIsBusy = true;
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

                MaterialMessageBox.ShowError(
                    $"{loginException.Message}{Environment.NewLine}Błąd: {loginException.InnerException?.Message}",
                    "Błąd");
            }
            catch (LoadingStartupDataException loadingStartupDataException)
            {
                _uiService.ResetMouseCursor();

                MaterialMessageBox.ShowError(
                    $"{loadingStartupDataException.Message}{Environment.NewLine}Błąd: {loadingStartupDataException.InnerException?.Message}",
                    "Błąd");
            }
            catch (UpdatingContractStatesException updatingContractException)
            {
                _uiService.ResetMouseCursor();

                MaterialMessageBox.ShowError(
                    $"{updatingContractException.Message}{Environment.NewLine}Błąd: {updatingContractException.InnerException.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _uiService.ResetMouseCursor();

                MaterialMessageBox.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
            finally
            {
                LoginButtonIsBusy = false;
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
            //return UserNameHasText && PasswordBoxHasText && !LoginButtonIsBusy;
            return true && !LoginButtonIsBusy; // For fast login while developing
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