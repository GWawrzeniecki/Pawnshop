using AutoMapper;
using PawnShop.Core.Interfaces;
using PawnShop.Modules.Worker.Base;
using PawnShop.Modules.Worker.Validators;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using System;
using System.Security;

namespace PawnShop.Modules.Worker.Dialogs.ViewModels
{
    public class LoginPrivilegesDataViewModel : WorkerDialogBase
    {
        #region PrivateMembers

        private readonly IHashService _hashService;
        private readonly IValidatorService _validatorService;
        private readonly IMessageBoxService _messageBoxService;
        private string _userLogin;
        private bool _baseTabs;
        private bool _workerTab;
        private bool _settingsTab;
        private bool _passwordBoxHasText;
        private string _passwordHash;
        private DelegateCommand<object> _passwordChangedCommand;
        private bool _passwordTag;

        #endregion

        #region Constructors
        public LoginPrivilegesDataViewModel(IMapper mapper, IHashService hashService, IValidatorService validatorService, IMessageBoxService messageBoxService, LoginPrivilegesDataViewModelValidator validator) : base(mapper, validator)
        {
            _hashService = hashService;
            _validatorService = validatorService;
            _messageBoxService = messageBoxService;
            Header = "Dane logowania i uprawnienia";
        }

        #endregion

        #region Commands

        public DelegateCommand<object> PasswordChangedCommand =>
            _passwordChangedCommand ??= new DelegateCommand<object>(PasswordChanged);


        #endregion

        #region PublicProperties

        public string UserLogin
        {
            get => _userLogin;
            set => SetProperty(ref _userLogin, value);
        }

        public string PasswordHash
        {
            get => _passwordHash;
            set => SetProperty(ref _passwordHash, value);
        }

        public bool BaseTabs
        {
            get => _baseTabs;
            set => SetProperty(ref _baseTabs, value);
        }

        public bool WorkerTab
        {
            get => _workerTab;
            set => SetProperty(ref _workerTab, value);
        }

        public bool SettingsTab
        {
            get => _settingsTab;
            set => SetProperty(ref _settingsTab, value);
        }

        public bool PasswordBoxHasText
        {
            get => _passwordBoxHasText;
            set
            {
                SetProperty(ref _passwordBoxHasText, value);
                RaisePropertyChanged(nameof(PasswordTag));
            }
        }
        public bool PasswordTag
        {
            get => _passwordTag;
            set => SetProperty(ref _passwordTag, value);
        }

        public override bool IsValidForm => !HasErrors && !string.IsNullOrEmpty(PasswordHash);

        public string FakePassword => "XXXXXXXX";


        #endregion

        #region CommandMethods

        private void PasswordChanged(object view)
        {
            if (view is not IHavePassword iHavePassword) return;

            using var pw = iHavePassword.Password.Copy();

            if (IsValidPassword(pw))
                HashNewPassword(pw);
        }

        #endregion

        #region PrivateMethods

        private void HashNewPassword(SecureString password)
        {
            try
            {
                PasswordHash = _hashService.Hash(password);
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError($"Wystąpił błąd podczas szyfrowania hasła.{Environment.NewLine}{e.Message}", "Wystąpił błąd");
                PasswordHash = null;
                RaiseAllCommands();
            }

        }

        private bool IsValidPassword(SecureString password)
        {
            var result = _validatorService.IsValidPassword(password);
            PasswordTag = result;
            return result;
        }

        #endregion
    }
}
