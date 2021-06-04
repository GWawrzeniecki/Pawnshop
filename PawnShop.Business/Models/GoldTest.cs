using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class GoldTest
    {
        public GoldTest()
        {
            GoldProducts = new HashSet<GoldProduct>();
        }

        public int Id { get; set; }
        public string GoldTest1 { get; set; }

        public virtual ICollection<GoldProduct> GoldProducts { get; set; }
    }
}
