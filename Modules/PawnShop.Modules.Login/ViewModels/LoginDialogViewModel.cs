using PawnShop.Business.Dtos;
using PawnShop.Core.Events;
using PawnShop.Core.Interfaces;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Login.Validators;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Services.Dialogs;
using System;
using System.Security;
using System.Threading.Tasks;


namespace PawnShop.Modules.Login.ViewModels
{
    public class LoginDialogViewModel : ViewModelBase<LoginDialogViewModel>, IDialogAware
    {
        #region private members

        private readonly ILoginService _loginService;
        private readonly IUIService _uiService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageBoxService _messageBoxService;
        private bool _userNameHasText;
        private bool _passwordBoxHasText;
        private bool _passwordTag;
        private string _userName;
        private DelegateCommand<object> _loginCommand;
        private bool _loginButtonIsBusy;

        #endregion private members

        #region public members

        public string Title => "Lombard \"VIP\"";

        public event Action<IDialogResult> RequestClose;

        #endregion public members

        #region constructor

        public LoginDialogViewModel(ILoginService loginService, IUIService uService, IEventAggregator eventAggregator, LoginDialogValidator loginDialogValidator, IMessageBoxService messageBoxService) : base(loginDialogValidator)
        {
            _loginService = loginService;
            _uiService = uService;
            _eventAggregator = eventAggregator;
            _messageBoxService = messageBoxService;
            PasswordTag = true;
        }

        #endregion constructor

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

        #region commands

        public DelegateCommand<object> LoginCommand =>
            _loginCommand ??= new DelegateCommand<object>(LoginAsync, CanLogin)
                .ObservesProperty(() => UserNameHasText)
                .ObservesProperty(() => PasswordBoxHasText)
                .ObservesProperty(() => LoginButtonIsBusy);

        #endregion commands

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

        private async void LoginAsync(object view)
        {
            try
            {
                LoginButtonIsBusy = true;
                var iHavePassword = view as IHavePassword;
                using var password = iHavePassword.Password.Copy();
                _uiService.SetMouseBusyCursor();
                var (success, loggedUser) = await TryToLoginAsync(UserName, password);
                if (success)
                {
                    await TryToStartStartupProcedures(loggedUser);
                    _eventAggregator.GetEvent<UserChangedEvent>().Publish();
                    CloseDialogWithSuccess();
                }

                _uiService.ResetMouseCursor();
            }
            catch (LoginException loginException)
            {
                _uiService.ResetMouseCursor();

                _messageBoxService.ShowError(
                    $"{loginException.Message}{Environment.NewLine}Błąd: {loginException.InnerException?.Message}",
                    "Błąd");
            }
            catch (LoadingStartupDataException loadingStartupDataException)
            {
                _uiService.ResetMouseCursor();

                _messageBoxService.ShowError(
                    $"{loadingStartupDataException.Message}{Environment.NewLine}Błąd: {loadingStartupDataException.InnerException?.Message}",
                    "Błąd");
            }
            catch (UpdatingContractStatesException updatingContractException)
            {
                _uiService.ResetMouseCursor();

                _messageBoxService.ShowError(
                    $"{updatingContractException.Message}{Environment.NewLine}Błąd: {updatingContractException.InnerException.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _uiService.ResetMouseCursor();

                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");

            }
            finally
            {
                LoginButtonIsBusy = false;
            }
        }

        private async Task TryToStartStartupProcedures(WorkerBossLoginDto loggedUser)
        {
            await _loginService.LoadStartupData(loggedUser);
            await _loginService.UpdateContractStates();
        }

        #endregion command methods

        #region private methods

        private bool CanLogin(object view)
        {
            return UserNameHasText && PasswordBoxHasText && !LoginButtonIsBusy;
        }

        private async Task<(bool, WorkerBossLoginDto)> TryToLoginAsync(string userName, SecureString password)
        {
            (bool success, WorkerBossLoginDto loggedUser) = await _loginService.LoginAsync(userName, password);
            PasswordTag = success;

            return (success, loggedUser);
        }

        private void CloseDialog(ButtonResult buttonResult) => RequestClose?.Invoke(new DialogResult(buttonResult));

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