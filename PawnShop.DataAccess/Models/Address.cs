using System;
using System.Collections.Generic;

#nullable disable

namespace PawnShop.EF.Models
{
    public partial class Address
    {
        public Address()
        {
            WorkPlaces = new HashSet<WorkPlace>();
        }

        public int AddressId { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string PostCode { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }

        public virtual City City { get; set; }
        public virtual Country Country { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<WorkPlace> WorkPlaces { get; set; }
    }
}
