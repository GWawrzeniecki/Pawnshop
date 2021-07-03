using PawnShop.Business.Models;
using Prism.Mvvm;

namespace PawnShop.Core.SharedVariables
{
    public class SessionContext : BindableBase, ISessionContext
    {
        #region private members

        private WorkerBoss _loggedPerson;
        private MoneyBalance _moneyBalance;
       


        #endregion private members

        #region public properties

        public WorkerBoss LoggedPerson
        {
            get => _loggedPerson;
            set => SetProperty(ref _loggedPerson, value);
        }

        public MoneyBalance TodayMoneyBalance
        {
            get => _moneyBalance;
            set => SetProperty(ref _moneyBalance, value);
        }



        #endregion public properties
    }
}