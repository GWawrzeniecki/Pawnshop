using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class Gemstone
    {
        public Gemstone()
        {
            GoldProductGemstones = new HashSet<GoldProductGemstone>();
        }

        public int GemstoneId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<GoldProductGemstone> GoldProductGemstones { get; set; }
    }
}
