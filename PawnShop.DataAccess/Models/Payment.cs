using System;
using System.Collections.Generic;

#nullable disable

namespace PawnShop.EF.Models
{
    public partial class Payment
    {
        public Payment()
        {
            DealDocuments = new HashSet<DealDocument>();
        }

        public int PaymentId { get; set; }
        public int PaymentTypeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual ICollection<DealDocument> DealDocuments { get; set; }
    }
}
