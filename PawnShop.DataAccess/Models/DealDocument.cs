using System.Collections.Generic;

#nullable disable

namespace PawnShop.DataAccess.Models
{
    public partial class DealDocument
    {
        public DealDocument()
        {
            ContractClientRenews = new HashSet<ContractClientRenew>();
        }

        public int DealDocumentId { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Income { get; set; }
        public decimal? RepaymentCapital { get; set; }
        public decimal? Profit { get; set; }
        public string ContractNumberId { get; set; }
        public int MoneyBalanceId { get; set; }
        public int PaymentId { get; set; }

        public virtual Contract ContractNumber { get; set; }
        public virtual MoneyBalance MoneyBalance { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual ICollection<ContractClientRenew> ContractClientRenews { get; set; }
    }
}