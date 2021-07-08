namespace PawnShop.Core.SharedVariables
{
    public interface ISessionContext
    {
        public WorkerBoss LoggedPerson { get; set; }
        public MoneyBalance TodayMoneyBalance { get; set; }
        public Business.Models.Contract ContractToRenew { get; set; }

    }
}