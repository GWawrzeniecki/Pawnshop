using PawnShop.Core.Interfaces;
using PawnShop.Modules.Worker.Base;
using PawnShop.Modules.Worker.Dialogs.ViewModels;
using System.Security;
using System.Windows.Controls;

namespace PawnShop.Modules.Worker.Dialogs.Views
{
    /// <summary>
    /// Interaction logic for LoginPrivilegesData
    /// </summary>
    public partial class LoginPrivilegesData : WorkerDialogViewBase, IHavePassword
    {
        public LoginPrivilegesData()
        {
            InitializeComponent();

        }

        #region IHavePassword

        public SecureString Password => PasswordBox.SecurePassword;

        public void SetFakePassword()
        {
            PasswordBox.Password = (this.DataContext as LoginPrivilegesDataViewModel).FakePassword;
        }

        #endregion



    }
}
