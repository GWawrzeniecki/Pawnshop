using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class Country
    {
        public Country()
        {
            Addresses = new HashSet<Address>();
            Cities = new HashSet<City>();
        }

        public int CountryId { get; set; }
        public string Country1 { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}
