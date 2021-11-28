using System;
using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class Client
    {
        public Client()
        {
            ContractBuyBacks = new HashSet<Contract>();
            ContractDealMakers = new HashSet<Contract>();
            ContractRenews = new HashSet<ContractRenew>();
            Payments = new HashSet<Payment>();
        }

        public int ClientId { get; set; }
        public string IdcardNumber { get; set; }
        public DateTime ValidityDateIdcard { get; set; }
        public string Pesel { get; set; }

        public virtual Person ClientNavigation { get; set; }
        public virtual ICollection<Contract> ContractBuyBacks { get; set; }
        public virtual ICollection<Contract> ContractDealMakers { get; set; }
        public virtual ICollection<ContractRenew> ContractRenews { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
