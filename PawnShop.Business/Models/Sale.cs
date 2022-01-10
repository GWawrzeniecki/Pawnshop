using System;
using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class Sale
    {
        public Sale()
        {
            Links = new HashSet<Link>();
        }

        public int SaleId { get; set; }
        public int ContractItemId { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime PutOnSaleDate { get; set; }
        public int Quantity { get; set; }
        public decimal? SoldPrice { get; set; }
        public DateTime? SaleDate { get; set; }
        public bool IsSold { get; set; }

        public virtual ContractItem ContractItem { get; set; }
        public virtual LocalSale LocalSale { get; set; }
        public virtual ICollection<Link> Links { get; set; }
    }
}
