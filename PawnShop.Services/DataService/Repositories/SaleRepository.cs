using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.DataAccess.Data;

namespace PawnShop.Services.DataService.Repositories
{
    public class SaleRepository : GenericRepository<SaleRepository>
    {
        public SaleRepository(PawnshopContext context) : base(context)
        {

        }

        public async Task<IList<Sale>> GetTopSales(SaleQueryData queryData, int count)
        {
            var query = GetTopSalesAsQueryable(count);

            query = query
                .Include(s => s.ContractItem)
                .ThenInclude(c => c.ContractNumber)
                .ThenInclude(cc => cc.DealMaker)
                .ThenInclude(d => d.ClientNavigation);

            if (!string.IsNullOrEmpty(queryData.Client))
            {
                query = query
                    .Where(s => (s.ContractItem.ContractNumber.DealMaker.ClientNavigation.FirstName + " " +
                                 s.ContractItem.ContractNumber.DealMaker.ClientNavigation.LastName)
                        .Contains(queryData.Client));
            }

            if (!string.IsNullOrEmpty(queryData.ContractNumber))
            {
                query = query.Where(s => s.ContractItem.ContractNumberId.Equals(queryData.ContractNumber));
            }

            if (queryData.FromDate.HasValue)
            {
                query = query.Where(p => DateTime.Compare(p.PutOnSaleDate, queryData.FromDate.Value) >= 0);
            }

            if (queryData.ToDate.HasValue)
            {
                query = query.Where(p => DateTime.Compare(p.PutOnSaleDate, queryData.ToDate.Value) <= 0);
            }

            if (decimal.TryParse(queryData.SalePrice, out decimal result))
            {
                if (queryData.PriceOption is not null)
                {
                    query = queryData.PriceOption.PriceOption switch
                    {
                        PriceOption.Equal => query.Where(s => s.SalePrice == result),
                        PriceOption.Lower => query.Where(s => s.SalePrice <= result),
                        PriceOption.Higher => query.Where(s => s.SalePrice >= result),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
                else
                {
                    query = query.Where(s => s.SalePrice == result);
                }
            }

            if (queryData.ContractItemCategory is not null)
            {
                query = query
                    .Where(s => s.ContractItem.CategoryId == queryData.ContractItemCategory.Id);
            }

            return await query
                .ToListAsync();
        }

        public async Task<IList<Sale>> GetTopSales(int count)
        {
            return await GetTopSalesAsQueryable(count)
                .ToListAsync();
        }

        private IQueryable<Sale> GetTopSalesAsQueryable(int count)
        {
            return context
                .Sales
                .Include(s => s.ContractItem)
                .ThenInclude(c => c.Category)
                .Include(s => s.ContractItem)
                .ThenInclude(c => c.ContractItemState)
                .Take(count)
                .AsQueryable();
        }

    }
}