using PawnShop.Business.Dtos;
using System.Security;
using System.Threading.Tasks;

namespace PawnShop.Services.Interfaces
{
    public interface ILoginService
    {
        public enum LoginResult { Success, Fail }

        public Task<(bool success, WorkerBossLoginDto loggedUser)> LoginAsync(string userName, SecureString password);

        public Task LoadStartupData(WorkerBossLoginDto loggedUser);

        public Task UpdateContractStates();

        public LoginResult ShowLoginDialog();

        public void ShowLogoutDialog();
    }
}