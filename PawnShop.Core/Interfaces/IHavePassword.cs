using System.Security;

namespace PawnShop.Core.Interfaces
{
    public interface IHavePassword
    {
        SecureString Password { get; }
        void SetFakePassword();
    }
}