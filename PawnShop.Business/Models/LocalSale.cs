#nullable disable

namespace PawnShop.Business.Models
{
    public partial class LocalSale
    {
        public int SaleId { get; set; }
        public string Rack { get; set; }
        public int Shelf { get; set; }

        public virtual Sale Sale { get; set; }
    }
}
