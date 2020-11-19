using System;
using System.Collections.Generic;

#nullable disable

namespace Pawnshop.DAL.Models
{
    public partial class Contract
    {
        public Contract()
        {
            ContractClientRenews = new HashSet<ContractClientRenew>();
            ContractItems = new HashSet<ContractItem>();
            DealDocuments = new HashSet<DealDocument>();
        }

        public string ContractNumberId { get; set; }
        public int LendingRateId { get; set; }
        public DateTime StartDate { get; set; }
        public decimal AmountContract { get; set; }
        public int DealMakerId { get; set; }
        public int? BuyBackId { get; set; }
        public int WorkerBossId { get; set; }
        public int ContractStateId { get; set; }

        public virtual Client BuyBack { get; set; }
        public virtual ContractState ContractState { get; set; }
        public virtual Client DealMaker { get; set; }
        public virtual LendingRate LendingRate { get; set; }
        public virtual WorkerBoss WorkerBoss { get; set; }
        public virtual EndedContract EndedContract { get; set; }
        public virtual ICollection<ContractClientRenew> ContractClientRenews { get; set; }
        public virtual ICollection<ContractItem> ContractItems { get; set; }
        public virtual ICollection<DealDocument> DealDocuments { get; set; }
    }
}
