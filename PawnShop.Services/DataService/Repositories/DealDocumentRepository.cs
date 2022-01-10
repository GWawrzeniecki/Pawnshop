using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Dtos;
using PawnShop.Business.Models;
using PawnShop.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Services.DataService.Repositories
{
    public class DealDocumentRepository : GenericRepository<DealDocument>
    {
        public DealDocumentRepository(PawnshopContext context) : base(context)
        {
        }

        #region PublicMethods

        public async Task<IList<DealDocumentRenewChartDto>> GetRenewSumInDays(DateTime startDate, DateTime endDate)
        {
            return await context.DealDocuments
                .Include(d => d.Payment)
                .Where(s => s.Income.HasValue && !s.Profit.HasValue && !s.Cost.HasValue && !s.RepaymentCapital.HasValue && s.Payment.ClientId.HasValue && DateTime.Compare(s.MoneyBalanceId, startDate) >= 0 &&
                            DateTime.Compare(s.MoneyBalanceId, endDate) <= 0)
                .GroupBy(s => s.MoneyBalanceId)
                .Select(s => new DealDocumentRenewChartDto()
                {
                    RenewDate = s.Key,
                    IncomeSum = s.Sum(s2 => s2.Income.Value)
                }).ToListAsync();
        }

        #endregion
    }
}