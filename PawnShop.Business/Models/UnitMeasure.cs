using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class UnitMeasure
    {
        public UnitMeasure()
        {
            ContractItemCategories = new HashSet<ContractItemCategory>();
        }

        public int Id { get; set; }
        public string Measure { get; set; }

        public virtual ICollection<ContractItemCategory> ContractItemCategories { get; set; }
    }
}
