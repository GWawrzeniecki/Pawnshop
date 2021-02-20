using System.Security;
using System.Windows.Controls;

namespace PawnShop.Modules.Login.Extensions
{
    public static class PasswordBoxExtensions
    {
        public static SecureString GetReadOnlyCopy(this PasswordBox passwordBox)
        {
            var passwordBoxCopy = passwordBox.SecurePassword.Copy();
            passwordBoxCopy.MakeReadOnly();
            return passwordBoxCopy;
        }
    }
}