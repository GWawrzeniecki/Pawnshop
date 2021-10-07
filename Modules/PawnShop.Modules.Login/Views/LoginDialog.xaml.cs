using PawnShop.Core.Interfaces;
using System.Security;
using System.Windows.Controls;

namespace PawnShop.Modules.Login.Views
{
    /// <summary>
    /// Interaction logic for LoginDialog
    /// </summary>
    public partial class LoginDialog : UserControl, IHavePassword
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        #region IHavePassword

        public SecureString Password => PasswordBox.SecurePassword;
        public void SetFakePassword()
        {

        }

        #endregion
    }
}