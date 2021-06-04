using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class GoldProduct
    {
        public GoldProduct()
        {
            GoldProductGemstones = new HashSet<GoldProductGemstone>();
        }

        public int ContractitemId { get; set; }
        public int GoldTestId { get; set; }
        public int TypeId { get; set; }
        public decimal Grammage { get; set; }
        public int? Carat { get; set; }

        public virtual ContractItem Contractitem { get; set; }
        public virtual GoldTest GoldTest { get; set; }
        public virtual GoldProductType Type { get; set; }
        public virtual ICollection<GoldProductGemstone> GoldProductGemstones { get; set; }
    }
}
