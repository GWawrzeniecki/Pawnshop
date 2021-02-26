using PawnShop.Business.Models;
using PawnShop.Core.SharedVariables;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using PawnShop.Exceptions.DBExceptions;
using System;
using System.Linq;
using System.Security;
using System.Threading.Tasks;


namespace PawnShop.Services.Implementations
{
    public class LoginService : ILoginService
    {
        #region private members

        private readonly IHashService _hashService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISessionContext _sessionContext;

        #endregion private members

        #region constructor

        public LoginService(IHashService hashService, IUnitOfWork unitOfWork, ISessionContext sessionContext)
        {
            this._hashService = hashService;
            this._unitOfWork = unitOfWork;
            this._sessionContext = sessionContext;
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
                throw new LoadingStartupDataException("Wystąpił problem podczas ładowania danych aplikacji.", e);
            }
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

            if (!success)
                return (false, null);

            return (_hashService.Check(workerBoss.Hash, password), workerBoss);
        }

        private async Task<(bool success, WorkerBoss workerBoss)> TryGetWorkerBossAsync(string login)
        {
            var workerBoss = (await _unitOfWork.WorkerBossRepository.GetAsync(filter: p => p.Login.Equals(login))).FirstOrDefault();

            if (workerBoss == null)
                return (false, workerBoss);

            return (true, workerBoss);
        }

        private async Task TryLoadStartupData(WorkerBoss loggedUser)
        {
            var loggedPerson = await _unitOfWork.PersonRepository.GetByIDAsync(loggedUser.WorkerBossId);
            await _unitOfWork.MoneyBalanceRepository.CreateTodayMoneyBalance();
            var todayMoneyBalance = await _unitOfWork.MoneyBalanceRepository.GetTodayMoneyBalanceAsync();
            _sessionContext.LoggedPerson = loggedPerson;
            _sessionContext.TodayMoneyBalance = todayMoneyBalance;
        }

        #endregion private method
    }
}