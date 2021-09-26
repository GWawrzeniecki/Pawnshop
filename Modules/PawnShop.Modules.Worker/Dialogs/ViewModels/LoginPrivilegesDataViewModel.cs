using AutoMapper;
using PawnShop.Core.Interfaces;
using PawnShop.Core.SharedVariables;
using PawnShop.Modules.Worker.Base;
using Prism.Commands;
using System.Security;

namespace PawnShop.Modules.Worker.Dialogs.ViewModels
{
    public class LoginPrivilegesDataViewModel : WorkerDialogBase
    {
        #region PrivateMembers

        private readonly IMapper _mapper;
        private readonly ISessionContext _sessionContext;
        private string _userLogin;
        private bool _baseTabs;
        private bool _workerTab;
        private bool _settingsTab;
        private bool _passwordBoxHasText;
        private bool _isRevealPasswordButtonVisible;
        private string _passwordHash;
        private DelegateCommand<object> _passwordChangedCommand;
        private SecureString _newPassword;

        #endregion

        #region Constructors
        public LoginPrivilegesDataViewModel(IMapper mapper, ISessionContext sessionContext) : base(mapper)
        {
            _mapper = mapper;
            _sessionContext = sessionContext;
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
            set => SetProperty(ref _passwordBoxHasText, value);
        }

        public bool IsRevealPasswordButtonVisible
        {
            get => _isRevealPasswordButtonVisible;
            set => SetProperty(ref _isRevealPasswordButtonVisible, value);
        }

        public string FakePassword => "XXXXXXXX";

        #endregion

        #region WorkerDialogBase

        protected override void MapWorkerBossToVm()
        {
            base.MapWorkerBossToVm();
        }

        #endregion

        #region CommandMethods

        private void PasswordChanged(object view)
        {
            if (view is IHavePassword iHavePassword)
            {
                _newPassword = iHavePassword.Password;
                _newPassword.MakeReadOnly();
            }
        }

        #endregion

        #region PrivateMethods

        #endregion
    }
}
