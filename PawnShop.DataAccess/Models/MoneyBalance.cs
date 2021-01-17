using System;
using System.Collections.Generic;

#nullable disable

namespace PawnShop.EF.Models
{
    public partial class MoneyBalance
    {
        public MoneyBalance()
        {
            DealDocuments = new HashSet<DealDocument>();
        }

        public int MoneyBalanceId { get; set; }
        public DateTime TodayDate { get; set; }
        public decimal MoneyBalanceBwork { get; set; }
        public decimal? MoneyBalanceAwork { get; set; }

        public virtual ICollection<DealDocument> DealDocuments { get; set; }
    }
}
