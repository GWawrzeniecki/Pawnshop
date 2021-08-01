using System;
using System.Collections.Generic;

namespace PawnShop.Services.DataService.InsertModels
{
    public class InsertContract
    {
        public string ContractNumberId { get; set; }
        public int LendingRateId { get; set; }
        public int ContractStateId { get; set; }
        public int DealMakerId { get; set; }
        public int? BuyBackId { get; set; }
        public int WorkerBossId { get; set; }
        public DateTime StartDate { get; set; }
        public decimal AmountContract { get; set; }

        public virtual IList<InsertContractItem> ContractItems { get; set; }
    }
}