using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.DataAccess.Data;
using System.Threading.Tasks;
using static PawnShop.Services.Constants;

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
    }
}
