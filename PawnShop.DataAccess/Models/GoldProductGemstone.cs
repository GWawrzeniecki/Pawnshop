#nullable disable

namespace PawnShop.DataAccess.Models
{
    public partial class GoldProductGemstone
    {
        public int GoldProductId { get; set; }
        public int GemstoneId { get; set; }
        public int Amount { get; set; }

        public virtual Gemstone Gemstone { get; set; }
        public virtual GoldProduct GoldProduct { get; set; }
    }
}