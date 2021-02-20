using System.Collections.Generic;

#nullable disable

namespace PawnShop.DataAccess.Models
{
    public partial class ContractState
    {
        public ContractState()
        {
            Contracts = new HashSet<Contract>();
        }

        public int Id { get; set; }
        public string State { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; }
    }
}