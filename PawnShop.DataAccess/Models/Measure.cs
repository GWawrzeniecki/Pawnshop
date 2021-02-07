using System.Collections.Generic;

#nullable disable

namespace PawnShop.EF.Models
{
    public partial class Measure
    {
        public Measure()
        {
            ContractItems = new HashSet<ContractItem>();
        }

        public int Id { get; set; }
        public string Measure1 { get; set; }

        public virtual ICollection<ContractItem> ContractItems { get; set; }
    }
}