using PawnShop.Business.Dtos;
using PawnShop.Business.Models;

namespace PawnShop.Core.SharedVariables
{
    public interface ISessionContext
    {
        public WorkerBossLoginDto LoggedPerson { get; set; }
        public MoneyBalance TodayMoneyBalance { get; set; }


    }
}