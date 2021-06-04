using System;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class DealDocument
    {
        public int DealDocumentId { get; set; }
        public DateTime MoneyBalanceId { get; set; }
        public int PaymentId { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Income { get; set; }
        public decimal? RepaymentCapital { get; set; }
        public decimal? Profit { get; set; }

        public virtual MoneyBalance MoneyBalance { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual ContractRenew ContractRenew { get; set; }
    }
}
