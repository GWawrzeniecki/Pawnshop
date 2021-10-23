using System;
using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class Contract
    {
        public Contract()
        {
            ContractItems = new HashSet<ContractItem>();
            ContractRenews = new HashSet<ContractRenew>();
        }

        public string ContractNumberId { get; set; }
        public int CreateContractDealDocumentId { get; set; }
        public int? BuyBackDealDocumentId { get; set; }
        public int LendingRateId { get; set; }
        public int ContractStateId { get; set; }
        public int DealMakerId { get; set; }
        public int? BuyBackId { get; set; }
        public int WorkerBossId { get; set; }
        public DateTime StartDate { get; set; }
        public decimal AmountContract { get; set; }

        public virtual Client BuyBack { get; set; }
        public virtual DealDocument BuyBackDealDocument { get; set; }
        public virtual ContractState ContractState { get; set; }
        public virtual DealDocument CreateContractDealDocument { get; set; }
        public virtual Client DealMaker { get; set; }
        public virtual LendingRate LendingRate { get; set; }
        public virtual WorkerBoss WorkerBoss { get; set; }
        public virtual ICollection<ContractItem> ContractItems { get; set; }
        public virtual ICollection<ContractRenew> ContractRenews { get; set; }
    }
}
