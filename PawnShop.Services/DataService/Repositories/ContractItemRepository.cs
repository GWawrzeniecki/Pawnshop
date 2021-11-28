﻿using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Services.DataService.Repositories
{
    public class ContractItemRepository : GenericRepository<ContractItem>
    {
        public ContractItemRepository(PawnshopContext context) : base(context)
        {

        }

        public async Task<IList<ContractItem>> GetTopContractItemsForSale(ContractItemQueryData queryData, int count)
        {
            var query = await GetTopContractItemForSaleAsQueryable(count);

            query = QueryContractItems(query, queryData);
            return await query
                .ToListAsync();
        }

        public async Task<IList<ContractItem>> GetTopContractItemsForSale(int count)
        {
            var query = await GetTopContractItemForSaleAsQueryable(count);
            return await query
                .ToListAsync();
        }

        public async Task<IList<ContractItem>> GetTopContractItemsNotForSale(ContractItemQueryData queryData, int count)
        {
            var query = await GetTopContractItemNotForSaleAsQueryableAsync(count);

            query = QueryContractItems(query, queryData);

            return await query
                .ToListAsync();
        }

        public async Task<IList<ContractItem>> GetTopContractItemsNotForSale(int count)
        {
            var query = await GetTopContractItemNotForSaleAsQueryableAsync(count);
            return await query
               .ToListAsync();
        }

        private async Task<IQueryable<ContractItem>> GetTopContractItemNotForSaleAsQueryableAsync(int count)
        {
            var boughtContractState = await context
                .ContractStates
                .FirstOrDefaultAsync(c => c.State.Equals("Wykupiona"));

            if (boughtContractState is null)
                throw new Exception("Nie znaleziono statusu umowy o nazwie: Wykupiona");

            var notBoughtContractState = await context
                .ContractStates
                .FirstOrDefaultAsync(c => c.State.Equals("Niewykupiona"));

            if (notBoughtContractState is null)
                throw new Exception("Nie znaleziono statusu umowy o nazwie: Niewykupiona");

            return context
                .ContractItems
                .Include(c => c.ContractItemState)
                .Include(c => c.Category)
                .ThenInclude(c => c.Measure)
                .Include(c => c.Laptop)
                .Include(c => c.Telephone)
                .Include(c => c.GoldProduct)
                .Where(c => c.ContractNumber.ContractStateId != boughtContractState.Id && c.ContractNumber.ContractStateId != notBoughtContractState.Id)
                .Take(count)
                .AsQueryable();
        }

        private async Task<IQueryable<ContractItem>> GetTopContractItemForSaleAsQueryable(int count)
        {
            var notBoughtContractState = await context
                .ContractStates
                .FirstOrDefaultAsync(c => c.State.Equals("Niewykupiona"));

            if (notBoughtContractState is null)
                throw new Exception("Nie znaleziono statusu umowy o nazwie: Niewykupiona");

            return context
                .ContractItems
                .Include(c => c.ContractItemState)
                .Include(c => c.Category)
                .ThenInclude(c => c.Measure)
                .Include(c => c.Laptop)
                .Include(c => c.Telephone)
                .Include(c => c.GoldProduct)
                .Include(c => c.Sales)
                .Where(c => c.ContractNumber.ContractStateId == notBoughtContractState.Id && c.Sales.Sum(s => s.Quantity) < c.Amount)
                .Take(count)
                .AsQueryable();
        }

        private IQueryable<ContractItem> QueryContractItems(IQueryable<ContractItem> query, ContractItemQueryData queryData)
        {
            query = query
                .Include(c => c.ContractNumber)
                .ThenInclude(cn => cn.DealMaker)
                .ThenInclude(d => d.ClientNavigation);

            if (!string.IsNullOrEmpty(queryData.Client))
            {
                query = query
                    .Where(s => (s.ContractNumber.DealMaker.ClientNavigation.FirstName + " " +
                                 s.ContractNumber.DealMaker.ClientNavigation.LastName)
                        .Contains(queryData.Client));
            }

            if (!string.IsNullOrEmpty(queryData.ContractNumber))
            {
                query = query.Where(s => s.ContractNumberId.Equals(queryData.ContractNumber));
            }

            if (queryData.FromDate.HasValue)
            {
                query = query.Where(p => DateTime.Compare(p.ContractNumber.StartDate, queryData.FromDate.Value) >= 0);
            }

            if (queryData.ToDate.HasValue)
            {
                query = query.Where(p => DateTime.Compare(p.ContractNumber.StartDate, queryData.ToDate.Value) <= 0);
            }

            if (decimal.TryParse(queryData.SalePrice, out decimal result))
            {
                if (queryData.PriceOption is not null)
                {
                    query = queryData.PriceOption.PriceOption switch
                    {
                        PriceOption.Equal => query.Where(s => s.ContractNumber.AmountContract == result),
                        PriceOption.Lower => query.Where(s => s.ContractNumber.AmountContract <= result),
                        PriceOption.Higher => query.Where(s => s.ContractNumber.AmountContract >= result),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
                else
                {
                    query = query.Where(s => s.ContractNumber.AmountContract == result);
                }
            }

            if (queryData.ContractItemCategory is not null)
            {
                query = query
                    .Where(s => s.CategoryId == queryData.ContractItemCategory.Id);
            }

            return query;
        }
    }
}