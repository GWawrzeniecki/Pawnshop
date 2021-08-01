using System;

namespace PawnShop.Services.DataService.InsertModels
{
    public class InsertContractRenew
    {
        public string ContractNumberId { get; set; }
        public int LendingRateId { get; set; }
        public int ClientId { get; set; }
        public DateTime StartDate { get; set; }
    }
}