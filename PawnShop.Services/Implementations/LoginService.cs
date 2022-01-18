using PawnShop.Business.Dtos;
using PawnShop.Core;
using PawnShop.Core.Constants;
using PawnShop.Core.Dialogs;
using PawnShop.Core.Extensions;
using PawnShop.Core.HamburgerMenu.Interfaces;
using PawnShop.Core.Regions;
using PawnShop.Core.SharedVariables;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using static PawnShop.Services.Interfaces.ILoginService;

namespace PawnShop.Services.Implementations
{
    public class LoginService : ILoginService
    {
        #region private members

        private readonly IHashService _hashService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionContext _sessionContext;
        private readonly IDialogService _dialogService;
        private readonly IModuleCatalog _moduleCatalog;
        private readonly IModuleManager _moduleManager;
        private readonly IEventAggregator _ea;
        private readonly IRegionManager _regionManager;
        private readonly IApplicationCommands _applicationCommands;

        #endregion private members

        #region constructor

        public LoginService(IHashService hashService, IUnitOfWork unitOfWork, ISessionContext sessionContext, IDialogService dialogService,
        IModuleCatalog moduleCatalog, IModuleManager moduleManager, IEventAggregator ea, IRegionManager regionManager, IApplicationCommands applicationCommands)
        {
            _hashService = hashService;
            _unitOfWork = unitOfWork;
            _sessionContext = sessionContext;
            _dialogService = dialogService;
            _moduleCatalog = moduleCatalog;
            _moduleManager = moduleManager;
            _ea = ea;
            _regionManager = regionManager;
            _applicationCommands = applicationCommands;
        }

        #endregion constructor

        #region public methods

        public async Task<(bool success, WorkerBossLoginDto loggedUser)> LoginAsync(string login, SecureString password)
        {
            try
            {
                return await TryLoginAsync(login, password);
            }
            catch (Exception e)
            {
                throw new LoginException("Wystąpił problem podczas logowania do aplikacji.", e);
            }
        }

        public async Task LoadStartupData(WorkerBossLoginDto loggedUser)
        {
            try
            {
                await TryLoadStartupData(loggedUser);
            }
            catch (Exception e)
            {
                throw new LoadingStartupDataException("Wystąpił błąd podczas ładowania danych niezbędnych do działania aplikacji.", e);
            }
        }

        public async Task UpdateContractStates()
        {
            try
            {
                await TryToUpdateContractStates();
            }
            catch (Exception e)
            {
                throw new UpdatingContractStatesException("Wystąpił problem podczas aktualizacji stanów umów.", e);
            }
        }

        public LoginResult ShowLoginDialog()
        {
            LoginResult loginResult = LoginResult.Fail;

            _dialogService.ShowLoginDialog(c =>
            {
                switch (c.Result)
                {
                    case ButtonResult.OK:
                        loginResult = LoginResult.Success;
                        break;

                    default:
                        Application.Current.Shutdown();
                        break;
                }
            });

            return loginResult;
        }

        public void ShowLogoutDialog()
        {
            Application.Current.MainWindow.Hide();
            NavigateToHomeScreen();

            _dialogService.ShowLoginDialog(c =>
            {
                if (c.Result == ButtonResult.OK)
                {
                    SubscribeModulesToEvent();
                    ReloadModules();
                    Application.Current.MainWindow.Show();
                }
                else
                    Application.Current.Shutdown(1);
            });
        }

        #endregion public methods

        #region private method

        private async Task<(bool success, WorkerBossLoginDto loggedUser)> TryLoginAsync(string login, SecureString password)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException($"'{nameof(login)}' cannot be null or whitespace", nameof(login));

            if (password == null || password.Length == 0)
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));

            var (success, workerBoss) = await TryGetWorkerBossAsync(login);

            return !success ? (false, null) : (_hashService.Check(workerBoss.Hash, password), workerBoss);
        }

        private async Task<(bool success, WorkerBossLoginDto workerBoss)> TryGetWorkerBossAsync(string login)
        {
            var workerBoss = await _unitOfWork.WorkerBossRepository.GetWorkerBossLogin(login);


            return workerBoss == null ? (false, null) : (true, workerBoss);
        }

        private async Task TryLoadStartupData(WorkerBossLoginDto loggedUser)
        {
            await _unitOfWork.MoneyBalanceRepository.CreateTodayMoneyBalance();
            var todayMoneyBalance = await _unitOfWork.MoneyBalanceRepository.GetTodayMoneyBalanceAsync();
            _sessionContext.LoggedPerson = loggedUser;
            _sessionContext.TodayMoneyBalance = todayMoneyBalance;
        }

        private async Task TryToUpdateContractStates()
        {
            await _unitOfWork.ContractRepository.UpdateContractStates();
        }

        private void SubscribeModulesToEvent()
        {
            foreach (var moduleVisibility in _regionManager.Regions[RegionNames.MenuRegion].Views.OfType<IModuleVisibility>())
            {
                moduleVisibility.Subscribe();
            }
        }

        private void ReloadModules()
        {
            foreach (var moduleInfo in _moduleCatalog.Modules.OrderModules())
            {
                if (moduleInfo.HasCurrentUserPrivilege(_sessionContext))
                {
                    moduleInfo.ShowModule(_moduleManager, _ea);
                }
                else
                {
                    moduleInfo.HideModule(_ea);
                }
            }
        }
        private void NavigateToHomeScreen()
        {
            _applicationCommands.SetMenuItemCommand.Execute(Constants.HomeModuleHomeViewName);
        }

        #endregion private method
    }
}