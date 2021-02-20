using System.Security;
using System.Threading.Tasks;

namespace PawnShop.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<bool> LoginAsync(string userName, SecureString password);
    }
}