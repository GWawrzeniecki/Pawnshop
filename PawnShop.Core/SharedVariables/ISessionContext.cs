using PawnShop.Business.Models;

namespace PawnShop.Core.SharedVariables
{
    public interface ISessionContext
    {
        public Person LoggedPerson { get; set; }
        public MoneyBalance TodayMoneyBalance { get; set; }
    }
}