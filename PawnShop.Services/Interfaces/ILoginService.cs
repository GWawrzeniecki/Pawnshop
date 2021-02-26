using PawnShop.Business.Models;
using System.Security;
using System.Threading.Tasks;

namespace PawnShop.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<(bool success ,WorkerBoss loggedUser)> LoginAsync(string userName, SecureString password);
        public Task LoadStartupData(WorkerBoss loggedUser);

    }
}