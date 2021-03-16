using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.DataAccess.Data;
using System.Threading.Tasks;
using static PawnShop.Services.Constants;
using System.Linq;
using System.Collections.Generic;

namespace PawnShop.Services.DataService.Repositories
{
    public class ContractRepository : GenericRepository<Contract>
    {
        private readonly PawnshopContext _context;
        private readonly string _updateContractStatesProcedureName = "UpdateContractStates";

        public ContractRepository(PawnshopContext context) : base(context)
        {
            _context = context;

        }

        public async Task UpdateContractStates() => await _context.Database.ExecuteSqlRawAsync($"Exec [{DBSchemaName}].[{_updateContractStatesProcedureName}]");

        public async Task<IList<Contract>> GetTopContractsAsync(int count)
        {
            return await _context.Contracts
                .Include(p => p.ContractState)
                .Include(p => p.LendingRate)
                .Include(p => p.DealMaker)
                .ThenInclude(p => p.ClientNavigation)
                .OrderByDescending(ctr => ctr.StartDate)
                .Take(count)
                .ToListAsync();
        }
    }
}
