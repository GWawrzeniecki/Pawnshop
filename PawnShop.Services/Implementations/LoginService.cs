using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
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

        #endregion private members

        #region constructor

        public LoginService(IHashService hashService, IUnitOfWork unitOfWork)
        {
            this._hashService = hashService;
            this._unitOfWork = unitOfWork;
        }

        #endregion constructor

        #region public methods

        public async Task<bool> LoginAsync(string login, SecureString password)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException($"'{nameof(login)}' cannot be null or whitespace", nameof(login));

            if (password == null || password.Length == 0)
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));

            var (success, passwordHash) = await TryGetHashFromDbAsync(login);

            if (!success)
                return false;

            return _hashService.Check(passwordHash, password);
        }

        #endregion public methods

        #region private method

        private async Task<(bool success, string passwordHash)> TryGetHashFromDbAsync(string login)
        {
            var passwordHash = (await _unitOfWork.WorkerBossReepository.GetAsync(filter: p => p.Login.Equals(login))).FirstOrDefault()?.Hash;

            if (passwordHash == null)
                return (false, passwordHash);

            return (true, passwordHash);
        }

        #endregion private method
    }
}