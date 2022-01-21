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
                .Where(client => EF.Functions.Like(client.ClientNavigation.LastName, $"{surname}%"))
                .Take(50)
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
            return await GetClientsAsQueryable()
                .Take(count)
                .ToListAsync();
        }

        public async Task<IList<Client>> GetClients(ClientQueryData clientQueryData, int count)
        {
            var query = GetClientsAsQueryable();

            if (!string.IsNullOrEmpty(clientQueryData.FirstName))
            {
                query = query.Where(c => c.ClientNavigation.FirstName.Equals(clientQueryData.FirstName));
            }

            if (!string.IsNullOrEmpty(clientQueryData.LastName))
            {
                query = query.Where(c => c.ClientNavigation.LastName.Equals(clientQueryData.LastName));
            }

            if (!string.IsNullOrEmpty(clientQueryData.Pesel))
            {
                query = query.Where(c => c.Pesel.Equals(clientQueryData.Pesel));
            }

            if (!string.IsNullOrEmpty(clientQueryData.IdCardNumber))
            {
                query = query.Where(c => c.IdcardNumber.Equals(clientQueryData.IdCardNumber));
            }

            if (!string.IsNullOrEmpty(clientQueryData.Street))
            {
                query = query.Where(c => EF.Functions.Like(c.ClientNavigation.Address.Street, $"{clientQueryData.Street}%"));
            }

            if (!string.IsNullOrEmpty(clientQueryData.ContractNumber))
            {
                query = query
                    .Include(q => q.ContractDealMakers)
                    .Where(c => c.ContractDealMakers.Any(co => co.ContractNumberId == clientQueryData.ContractNumber));
            }

            return await query
                        .Take(count)
                        .ToListAsync();
        }

        #endregion

        #region PrivateMethods
        private IQueryable<Client> GetClientsAsQueryable()
        {
            return _context.Clients
                .Include(client => client.ClientNavigation)
                .ThenInclude(person => person.Address)
                .ThenInclude(address => address.City)
                .Include(client => client.ClientNavigation)
                .ThenInclude(person => person.Address)
                .ThenInclude(address => address.Country)
                .AsQueryable();
        }

        #endregion
    }
}