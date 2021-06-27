using System;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int PaymentTypeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int? ClientId { get; set; }

        public virtual Client Client { get; set; }
        public virtual PaymentType PaymentType { get; set; }
        public virtual DealDocument DealDocument { get; set; }
    }
}
