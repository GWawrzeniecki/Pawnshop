using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.Core.Models.QueryDataModels;
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

        public async Task<IList<Client>> GetClients(int count)
        {
            return await GetClientsAsQueryable(count)
                        .ToListAsync();
        }

        public async Task<IList<Client>> GetClients(ClientQueryData clientQueryData, int count)
        {
            var query = GetClientsAsQueryable(count);

            if (!string.IsNullOrEmpty(clientQueryData.FirstName))
            {
                query = query.Where(c => c.ClientNavigation.FirstName.Contains(clientQueryData.FirstName));
            }

            if (!string.IsNullOrEmpty(clientQueryData.LastName))
            {
                query = query.Where(c => c.ClientNavigation.LastName.Contains(clientQueryData.LastName));
            }

            if (!string.IsNullOrEmpty(clientQueryData.Pesel))
            {
                query = query.Where(c => c.Pesel.Contains(clientQueryData.Pesel));
            }

            if (!string.IsNullOrEmpty(clientQueryData.IdCardNumber))
            {
                query = query.Where(c => c.IdcardNumber.Contains(clientQueryData.IdCardNumber));
            }

            if (!string.IsNullOrEmpty(clientQueryData.Street))
            {
                query = query.Where(c => c.ClientNavigation.Address.Street.Contains(clientQueryData.Street));
            }

            if (!string.IsNullOrEmpty(clientQueryData.ContractNumber))
            {
                query = query
                    .Include(q => q.ContractDealMakers)
                    .Where(c => c.ContractDealMakers.Any(co => co.ContractNumberId == clientQueryData.ContractNumber));
            }

            return await query
                        .ToListAsync();
        }

        #endregion

        #region PrivateMethods

        public IQueryable<Client> GetClientsAsQueryable(int count)
        {
            return _context.Clients
                .Include(client => client.ClientNavigation)
                .ThenInclude(person => person.Address)
                .ThenInclude(address => address.City)
                .Include(client => client.ClientNavigation)
                .ThenInclude(person => person.Address)
                .ThenInclude(address => address.Country)
                .Take(count)
                .AsQueryable();
        }

        #endregion
    }
}