#nullable disable

namespace PawnShop.Business.Models
{
    public partial class Link
    {
        public int LinkId { get; set; }
        public string Link1 { get; set; }
        public int SaleId { get; set; }

        public virtual Sale Sale { get; set; }
    }
}
