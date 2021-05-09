using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.DataAccess.Data;

namespace PawnShop.Services.DataService.Repositories
{
    public class ClientRepository : GenericRepository<Client>
    {
        private readonly PawnshopContext _context;

        public ClientRepository(PawnshopContext context) : base(context)
        {
            _context = context;
        }

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
            var modifiedCountryProperty = _context.ChangeTracker
                .Entries<Country>()
                .Where(e => e.State == EntityState.Modified)
                .FirstOrDefault(e => e.Entity.CountryId == client.ClientNavigation.Address.CountryId)
                ?.Properties
                .FirstOrDefault(prop =>
                    prop.IsModified &&
                    prop.Metadata.Name.Equals(nameof(client.ClientNavigation.Address.Country.Country1)));


            var modifiedCityProperty = _context.ChangeTracker
                .Entries<City>()
                .Where(e => e.State == EntityState.Modified)
                .FirstOrDefault(e => e.Entity.CityId == client.ClientNavigation.Address.CityId)
                ?.Properties
                .FirstOrDefault(prop =>
                    prop.IsModified && prop.Metadata.Name.Equals(nameof(client.ClientNavigation.Address.City.City1)));

            var modifiedCityValue = modifiedCityProperty?.CurrentValue;
            var modifiedCountryValue = modifiedCountryProperty?.CurrentValue;

            if (modifiedCityValue != null && modifiedCountryValue == null)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(c =>
                    c.City1.Equals(modifiedCityValue));

                modifiedCityProperty.IsModified = false;

                if (city != null)
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

            else if (modifiedCountryValue != null && modifiedCityValue != null)
            {
                var country = await _context.Countries.FirstOrDefaultAsync(c =>
                    c.Country1.Equals(modifiedCountryValue));

                var city = await _context.Cities.FirstOrDefaultAsync(c =>
                    c.City1.Equals(modifiedCityValue));

                modifiedCityProperty.IsModified = false;
                modifiedCountryProperty.IsModified = false;

                if (city != null && country != null)
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
    }
}