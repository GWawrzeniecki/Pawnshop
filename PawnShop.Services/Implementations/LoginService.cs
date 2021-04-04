using PawnShop.Business.Models;
using PawnShop.Core.Dialogs;
using PawnShop.Core.SharedVariables;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
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

        #endregion private members

        #region constructor

        public LoginService(IHashService hashService, IUnitOfWork unitOfWork, ISessionContext sessionContext, IDialogService dialogService)
        {
            this._hashService = hashService;
            this._unitOfWork = unitOfWork;
            this._sessionContext = sessionContext;
            this._dialogService = dialogService;
        }

        #endregion constructor

        #region public methods

        public async Task<(bool success, WorkerBoss loggedUser)> LoginAsync(string login, SecureString password)
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

        public async Task LoadStartupData(WorkerBoss loggedUser)
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

            _dialogService.ShowLoginDialog(c =>
            {
                if (c.Result == ButtonResult.OK)
                    Application.Current.MainWindow.Show();
                else
                    Application.Current.Shutdown(1);
            });
        }

        #endregion public methods

        #region private method

        private async Task<(bool success, WorkerBoss loggedUser)> TryLoginAsync(string login, SecureString password)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException($"'{nameof(login)}' cannot be null or whitespace", nameof(login));

            if (password == null || password.Length == 0)
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));

            var (success, workerBoss) = await TryGetWorkerBossAsync(login);

            return !success ? (false, null) : (_hashService.Check(workerBoss.Hash, password), workerBoss);
        }

        private async Task<(bool success, WorkerBoss workerBoss)> TryGetWorkerBossAsync(string login)
        {
            var workerBoss = (await _unitOfWork.WorkerBossRepository.GetAsync(filter: p => p.Login.Equals(login))).FirstOrDefault();

            return workerBoss == null ? (false, null) : (true, workerBoss);
        }

        private async Task TryLoadStartupData(WorkerBoss loggedUser)
        {
            var loggedPerson = await _unitOfWork.PersonRepository.GetByIDAsync(loggedUser.WorkerBossId);
            await _unitOfWork.MoneyBalanceRepository.CreateTodayMoneyBalance();
            var todayMoneyBalance = await _unitOfWork.MoneyBalanceRepository.GetTodayMoneyBalanceAsync();
            _sessionContext.LoggedPerson = loggedPerson;
            _sessionContext.TodayMoneyBalance = todayMoneyBalance;
        }

        private async Task TryToUpdateContractStates()
        {
            await _unitOfWork.ContractRepository.UpdateContractStates();
        }

        #endregion private method
    }
}