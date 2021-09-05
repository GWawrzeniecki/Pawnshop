using PawnShop.Business.Models;
using System;

namespace PawnShop.Services.DataService.InsertModels
{
    public class InsertClient
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string ApartmentNumber { get; set; }
        public string PostCode { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public City City { get; set; }
        public Country Country { get; set; }

        public string IdcardNumber { get; set; }
        public DateTime ValidityDateIdcard { get; set; }
        public string Pesel { get; set; }
    }
}