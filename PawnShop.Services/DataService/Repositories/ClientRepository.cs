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

        #endregion
    }
}