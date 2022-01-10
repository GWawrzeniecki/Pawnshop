using System.Security;

namespace PawnShop.Services.Interfaces
{
    public interface IValidatorService
    {
        public bool ValidateIdCardNumber(string idCardNumber);
        public bool IsValidPassword(SecureString secureString);
    }
}