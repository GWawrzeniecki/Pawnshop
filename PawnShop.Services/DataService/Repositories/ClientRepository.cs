using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.DataAccess.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Services.DataService.Repositories
{
    public class ClientRepository : GenericRepository<Client>
    {
        #region Private members
        private readonly PawnshopContext _context;
        #endregion
        #region Constructor
        public ClientRepository(PawnshopContext context) : base(context)
        {
            _context = context;
        }
        #endregion

        #region Public methods
        public async Task<IList<Client>> GetClientBySurname(string surname)
        {
            return await _context.Clients
                .Include(client => client.ClientNavigation)
                .ThenInclude(person => person.Address)
                .ThenInclude(address => address.City)
                .Include(client => client.ClientNavigation)
                .ThenInclude(person => person.Address)
                .ThenInclude(address => address.Country)
                .Where(client => client.ClientNavigation.LastName.Contains(surname))
                .ToListAsync();
        }

        public async Task<IList<Client>> GetClientByPesel(string pesel)
        {
            return await _context.Clients
                .Include(client => client.ClientNavigation)
                .ThenInclude(person => person.Address)
                .ThenInclude(address => address.City)
                .Include(client => client.ClientNavigation)
                .ThenInclude(person => person.Address)
                .ThenInclude(address => address.Country)
                .Where(client => client.Pesel.Equals(pesel))
                .ToListAsync();
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c =>
                c.Country1.Equals(client.ClientNavigation.Address.Country.Country1));


            var city = await _context.Cities.FirstOrDefaultAsync(c =>
                c.City1.Equals(client.ClientNavigation.Address.City.City1));


            if (city != null && country != null)
            {
                client.ClientNavigation.Address.CityId = city.CityId;
                client.ClientNavigation.Address.City = city;
                client.ClientNavigation.Address.CountryId = country.CountryId;
                client.ClientNavigation.Address.Country = country;
            }

            if (country != null && city == null)
            {
                client.ClientNavigation.Address.CountryId = country.CountryId;
                client.ClientNavigation.Address.Country = country;

                client.ClientNavigation.Address.City.Country = country;
                client.ClientNavigation.Address.City.CountryId = country.CountryId;
            }

            if (country == null && city != null)
            {
                //the same city another country?
            }

            if (country == null && city == null)
            {
                client.ClientNavigation.Address.City.Country = client.ClientNavigation.Address.Country;
                client.ClientNavigation.Address.Country.Cities.Add(client.ClientNavigation.Address.City);
            }

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client> UpdateClientAsync(Client client)
        {
            var modifiedCountryProperty = GetModifiedPropertyEntry<Country, int>(x => x.CountryId, client.ClientNavigation.Address.CountryId, nameof(client.ClientNavigation.Address.Country.Country1));
            var modifiedCityProperty = GetModifiedPropertyEntry<City, int>(x => x.CityId, client.ClientNavigation.Address.CityId, nameof(client.ClientNavigation.Address.City.City1));

            var modifiedCityValue = modifiedCityProperty?.CurrentValue;
            var modifiedCountryValue = modifiedCountryProperty?.CurrentValue;

            bool IsNewCityAndOldCountry() => modifiedCityValue != null && modifiedCountryValue == null;
            bool IsNewCityAndNewCountry() => modifiedCityValue != null && modifiedCountryValue != null;

            if (IsNewCityAndOldCountry())
            {
                var city = await _context.Cities.FirstOrDefaultAsync(c =>
                    c.City1.Equals(modifiedCityValue));

                bool IsOldCity() => city != null;

                modifiedCityProperty.IsModified = false;

                if (IsOldCity())
                {
                    client.ClientNavigation.Address.CityId = city.CityId;
                    client.ClientNavigation.Address.City = city;

                }
                else
                {

                    var newCity = new City()
                    {
                        City1 = modifiedCityValue.ToString(),
                        Country = client.ClientNavigation.Address.Country,
                        CountryId = client.ClientNavigation.Address.CountryId
                    };

                    client.ClientNavigation.Address.CityId = 0;
                    client.ClientNavigation.Address.City = newCity;


                }
            }

            else if (IsNewCityAndNewCountry())
            {
                var country = await _context.Countries.FirstOrDefaultAsync(c =>
                    c.Country1.Equals(modifiedCountryValue));

                var city = await _context.Cities.FirstOrDefaultAsync(c =>
                    c.City1.Equals(modifiedCityValue));

                bool IsOldCityAndOldCountry() => country != null && city != null;

                modifiedCityProperty.IsModified = false;
                modifiedCountryProperty.IsModified = false;

                if (IsOldCityAndOldCountry())
                {
                    client.ClientNavigation.Address.CityId = city.CityId;
                    client.ClientNavigation.Address.City = city;
                    client.ClientNavigation.Address.CountryId = country.CountryId;
                    client.ClientNavigation.Address.Country = country;

                }

                else if (country != null)
                {
                    client.ClientNavigation.Address.CountryId = country.CountryId;
                    client.ClientNavigation.Address.Country = country;

                    var newCity = new City()
                    { City1 = modifiedCityValue.ToString(), Country = country, CountryId = country.CountryId };

                    client.ClientNavigation.Address.City = newCity;
                }

                else if (city != null)
                {
                    //the same city another country?
                }
                else
                {
                    var newCountry = new Country()
                    {
                        Country1 = modifiedCountryValue.ToString()
                    };

                    var newCity = new City()
                    {
                        City1 = modifiedCityValue.ToString(),
                        Country = newCountry
                    };

                    client.ClientNavigation.Address.Country = newCountry;
                    client.ClientNavigation.Address.City = newCity;
                }
            }


            await _context.SaveChangesAsync();
            return client;
        }
        #endregion
    }
}