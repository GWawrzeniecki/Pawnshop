using System.Security;

namespace PawnShop.Services.Interfaces
{
    public interface IHashService
    {
        string Hash(SecureString password);
        bool Check(string hash, SecureString password);
    }
}