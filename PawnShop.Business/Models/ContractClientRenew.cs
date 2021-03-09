﻿#nullable disable

namespace PawnShop.Business.Models
{
    public partial class ContractClientRenew
    {
        public int RenewContractId { get; set; }
        public string ContractNumberId { get; set; }
        public int ClientId { get; set; }
        public int DealDocumentId { get; set; }

        public virtual Client Client { get; set; }
        public virtual Contract ContractNumber { get; set; }
        public virtual DealDocument DealDocument { get; set; }
    }
}