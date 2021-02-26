using System;
using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class MoneyBalance
    {
        public MoneyBalance()
        {
            DealDocuments = new HashSet<DealDocument>();
        }

        public DateTime TodayDate { get; set; }
        public decimal MoneyBalance1 { get; set; }

        public virtual ICollection<DealDocument> DealDocuments { get; set; }
    }
}
