using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Dtos;
using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.DataAccess.Data;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Services.DataService.Repositories
{
    public class SaleRepository : GenericRepository<Sale>
    {
        public SaleRepository(PawnshopContext context) : base(context)
        {

        }

        public async Task<IList<Sale>> GetTopSales(ContractItemQueryData queryData, int count)
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
                .ThenInclude(s => s.Sales)
                .Include(s => s.ContractItem)
                .ThenInclude(c => c.Category)
                .Include(s => s.ContractItem)
                .ThenInclude(c => c.ContractItemState)
                .Include(s => s.ContractItem)
                .ThenInclude(c => c.Category)
                .ThenInclude(cc => cc.Measure)
                .Include(s => s.LocalSale)
                .Include(s => s.Links)
                .Include(s => s.ContractItem)
                .ThenInclude(s => s.Laptop)
                .Include(s => s.ContractItem)
                .ThenInclude(s => s.Telephone)
                .Include(s => s.ContractItem)
                .ThenInclude(s => s.GoldProduct)
                .Take(count)
                .AsQueryable();
        }

        public async Task Sell(Sale sale, decimal soldPrice, PaymentType paymentType, IContainerProvider containerProvider, decimal profit)
        {
            context.Sales.Attach(sale);
            sale.SoldPrice = soldPrice;
            sale.IsSold = true;
            sale.SaleDate = DateTime.Today;
            var payment = new Payment { PaymentTypeId = paymentType.Id, Amount = soldPrice, Date = DateTime.Today };
            using var unitOfWork = containerProvider.Resolve<IUnitOfWork>();
            var moneyBalance = await unitOfWork.MoneyBalanceRepository.GetTodayMoneyBalanceAsync();
            var dealDocument = new DealDocument { MoneyBalanceId = moneyBalance.TodayDate, Payment = payment, RepaymentCapital = soldPrice, Profit = profit };
            await context.DealDocuments.AddAsync(dealDocument);
            await context.SaveChangesAsync();
        }

        public async Task<IList<SaleChartDto>> GetSaleInDays(DateTime startDate, DateTime endDate)
        {
            return await context.Sales
                .Where(s => s.IsSold && DateTime.Compare(s.SaleDate.Value, startDate) >= 0 &&
                            DateTime.Compare(s.SaleDate.Value, endDate) <= 0)
                .GroupBy(s => s.SaleDate)
                .Select(s => new SaleChartDto()
                {
                    SaleDate = s.Key.Value,
                    SoldPriceSum = s.Sum(s2 => s2.SoldPrice.Value)
                }).ToListAsync();
        }

    }
}