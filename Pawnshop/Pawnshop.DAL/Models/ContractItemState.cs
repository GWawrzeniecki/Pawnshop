using System;
using System.Collections.Generic;

#nullable disable

namespace Pawnshop.DAL.Models
{
    public partial class ContractItemState
    {
        public ContractItemState()
        {
            ContractItems = new HashSet<ContractItem>();
        }

        public int Id { get; set; }
        public string State { get; set; }

        public virtual ICollection<ContractItem> ContractItems { get; set; }
    }
}
