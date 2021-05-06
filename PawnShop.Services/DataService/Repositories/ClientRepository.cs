using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                .Where(client => client.ClientNavigation.LastName.Contains(surname))
                .ToListAsync();
        }

        public async Task<IList<Client>> GetClientByPesel(string pesel)
        {
            return await _context.Clients
                .Include(client => client.ClientNavigation)
                .ThenInclude(person => person.Address)
                .ThenInclude(address => address.City)
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
            await _context.SaveChangesAsync();
            return client;
        }
    }
}