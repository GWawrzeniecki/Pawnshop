﻿using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class ContractItemCategory
    {
        public ContractItemCategory()
        {
            ContractItems = new HashSet<ContractItem>();
        }

        public int Id { get; set; }
        public string Category { get; set; }

        public virtual ICollection<ContractItem> ContractItems { get; set; }
    }
}