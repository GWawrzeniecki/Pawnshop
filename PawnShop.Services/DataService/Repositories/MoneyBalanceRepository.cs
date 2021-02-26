using Microsoft.Data.SqlClient;
using PawnShop.Business.Data;
using PawnShop.Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace PawnShop.Services.DataService.Repositories
{
    public class MoneyBalanceRepository : GenericRepository<MoneyBalance>
    {
        private PawnshopContext _context;
        private readonly string _getTodayMoneyBalanceProcedureName = "CreateTodayMoneyBalance";

        public MoneyBalanceRepository(PawnshopContext context) : base(context)
        {
            _context = context;
        }

        public async Task CreateTodayMoneyBalance() => await _context.Database.ExecuteSqlRawAsync($"Exec [Pawnshop].[{_getTodayMoneyBalanceProcedureName}]");


        public async Task<MoneyBalance> GetTodayMoneyBalanceAsync()
        {
            return await _context
                  .MoneyBalances
                 .OrderByDescending(mb => mb.TodayDate)
                 .FirstAsync();
        }
    }
}
