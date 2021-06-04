using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class City
    {
        public City()
        {
            Addresses = new HashSet<Address>();
        }

        public int CityId { get; set; }
        public int CountryId { get; set; }
        public string City1 { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
