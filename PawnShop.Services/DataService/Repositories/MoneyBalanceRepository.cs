using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.DataAccess.Data;
using System.Linq;
using System.Threading.Tasks;
using static PawnShop.Core.Constants.Constants;

namespace PawnShop.Services.DataService.Repositories
{
    public class MoneyBalanceRepository : GenericRepository<MoneyBalance>
    {
        private readonly PawnshopContext _context;
        private readonly string _getTodayMoneyBalanceProcedureName = "CreateTodayMoneyBalance";

        public MoneyBalanceRepository(PawnshopContext context) : base(context)
        {
            _context = context;
        }

        public async Task CreateTodayMoneyBalance() => await _context.Database.ExecuteSqlRawAsync($"Exec [{ProceduresSchemaName}].[{_getTodayMoneyBalanceProcedureName}]");

        public async Task<MoneyBalance> GetTodayMoneyBalanceAsync()
        {
            return await _context
                  .MoneyBalances
                 .OrderByDescending(mb => mb.TodayDate)
                 .FirstAsync();
        }

        public MoneyBalance GetTodayMoneyBalance()
        {
            return _context
                .MoneyBalances
                .OrderByDescending(mb => mb.TodayDate)
                .First();
        }
    }
}