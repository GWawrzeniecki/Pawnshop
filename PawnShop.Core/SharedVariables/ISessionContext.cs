using PawnShop.Business.Models;

namespace PawnShop.Core.SharedVariables
{
    public interface ISessionContext
    {
        public WorkerBoss LoggedPerson { get; set; }
        public MoneyBalance TodayMoneyBalance { get; set; }

       
    }
}