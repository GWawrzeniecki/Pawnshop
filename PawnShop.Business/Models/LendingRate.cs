using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class LendingRate
    {
        public LendingRate()
        {
            ContractRenews = new HashSet<ContractRenew>();
            Contracts = new HashSet<Contract>();
        }

        public int Id { get; set; }
        public int Procent { get; set; }
        public int Days { get; set; }

        public virtual ICollection<ContractRenew> ContractRenews { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
    }
}
