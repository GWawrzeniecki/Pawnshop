using PawnShop.Business.Models;
using Prism.Mvvm;

namespace PawnShop.Core.SharedVariables
{
    public class SessionContext : BindableBase, ISessionContext
    {
        #region private members

        private Person _loggedPerson;
        private MoneyBalance _moneyBalance;

        #endregion private members

        #region public properties

        public Person LoggedPerson
        {
            get { return _loggedPerson; }
            set { SetProperty(ref _loggedPerson, value); }
        }

        public MoneyBalance TodayMoneyBalance
        {
            get { return _moneyBalance; }
            set { SetProperty(ref _moneyBalance, value); }
        }

        #endregion public properties
    }
}