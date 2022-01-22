using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.Core.Extensions;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.DataAccess.Data;
using PawnShop.Services.DataService.InsertModels;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PawnShop.Core.Constants.Constants;


namespace PawnShop.Services.DataService.Repositories
{
    public class ContractRepository : GenericRepository<Contract>
    {
        private readonly PawnshopContext _context;
        private readonly IContainerProvider _containerProvider;
        private readonly IMapper _mapper;
        private readonly string _updateContractStatesProcedureName = "UpdateContractStates";

        public ContractRepository(PawnshopContext context, IContainerProvider containerProvider) : base(context)
        {
            _context = context;
            _containerProvider = containerProvider;
            _mapper = _containerProvider.Resolve<IMapper>();
        }

        public async Task UpdateContractStates() => await _context.Database.ExecuteSqlRawAsync($"Exec [{ProceduresSchemaName}].[{_updateContractStatesProcedureName}]");

        public async Task<string> GetNextContractNumber()
        {
            var actualContractNumber = await _context
                .Contracts
                .Where(c => c.StartDate == _context.Contracts.Max(c2 => c2.StartDate))
                .Select(c => new
                {
                    ContractNumberID = c.ContractNumberId,
                    Year = Convert.ToInt32(c.ContractNumberId.Substring(c.ContractNumberId.IndexOf("/") + 1, c.ContractNumberId.Length - c.ContractNumberId.IndexOf("/") + 1)),
                    Number = Convert.ToInt32(c.ContractNumberId.Substring(0, c.ContractNumberId.IndexOf("/")))
                })
                .OrderByDescending(c => c.Year)
                .ThenByDescending(c => c.Number)
                .Take(1)
                .ToListAsync();

            if (actualContractNumber == null || !actualContractNumber.Any())
                return $"01/{DateTime.Now.Year}";

            return actualContractNumber.First().ContractNumberID.GetNextContractNumber();
        }

        public async Task<IList<Contract>> GetTopContractsAsync(Client client, int count)
        {
            return await GetTopContractsQueryable(count)
                .Where(c => c.DealMakerId == client.ClientId)
                .ToListAsync();
        }

        public async Task<IList<Contract>> GetTopContractsAsync(int count)
        {
            return await GetTopContractsQueryable(count)
                        .ToListAsync();

        }

        public async Task<IList<Contract>> GetContracts(ContractQueryData queryData, int count)
        {
            var contractQuery = GetContractsQueryable();

            if (!string.IsNullOrEmpty(queryData.Client))
            {
                contractQuery = contractQuery
                    .Where(p => EF.Functions.Like(
                        p.DealMaker.ClientNavigation.FirstName + " " + p.DealMaker.ClientNavigation.LastName,
                        $"%{queryData.Client}%"));
            }

            if (!string.IsNullOrEmpty(queryData.ContractAmount))
            {
                if (decimal.TryParse(queryData.ContractAmount, out var contractAmount))
                {
                    contractQuery = contractQuery.Where(p => p.AmountContract == contractAmount);
                }
            }

            if (!string.IsNullOrEmpty(queryData.ContractNumber))
            {
                contractQuery = contractQuery.Where(p => p.ContractNumberId.Equals(queryData.ContractNumber));
            }

            if (queryData.FromDate.HasValue)
            {
                contractQuery = contractQuery.Where(p => DateTime.Compare(p.StartDate, queryData.FromDate.Value) >= 0);
            }

            if (queryData.ToDate.HasValue)
            {
                contractQuery = contractQuery.Where(p => DateTime.Compare(p.StartDate, queryData.ToDate.Value) <= 0);
            }

            if (queryData.ContractState != null)
            {
                contractQuery = contractQuery.Where(p => p.ContractStateId == queryData.ContractState.Id);
            }

            if (queryData.LendingRate != null)
            {
                contractQuery = contractQuery.Where(p => p.LendingRateId == queryData.LendingRate.Id);
            }

            //if (!string.IsNullOrEmpty(queryData.ContractNumber))
            //{
            //    return await contractQuery
            //        .ToListAsync();
            //}

            return await contractQuery
                .Take(count)
                .ToListAsync();
        }



        public async Task<Contract> CreateContract(InsertContract insertContract, string paymentTypeStr, decimal paymentAmount,
           DateTime paymentDate, decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default)
        {
            var contract = _mapper.Map<Contract>(insertContract);
            var paymentType = await _context.PaymentTypes.AsNoTracking().FirstOrDefaultAsync(p => p.Type.Equals(paymentTypeStr));
            var contractState = await _context.ContractStates.AsNoTracking().FirstOrDefaultAsync(c => c.State.Equals(CreatedContractState));
            var payment = new Payment { PaymentTypeId = paymentType.Id, Amount = paymentAmount, Date = paymentDate };
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            var moneyBalance = await unitOfWork.MoneyBalanceRepository.GetTodayMoneyBalanceAsync();
            var dealDocument = new DealDocument { MoneyBalanceId = moneyBalance.TodayDate, Payment = payment, Cost = cost, Income = income, RepaymentCapital = repaymentCapital, Profit = profit };
            contract.ContractStateId = contractState.Id;
            contract.CreateContractDealDocument = dealDocument;
            await _context.Contracts.AddAsync(contract);
            await _context.SaveChangesAsync();
            return contract;
        }


        public async Task<Contract> RenewContract(Contract contractToRenew, InsertContractRenew insertContractRenew, PaymentType paymentType, decimal paymentAmount,
            decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default)
        {
            var payment = new Payment { PaymentTypeId = paymentType.Id, Amount = paymentAmount, Date = DateTime.Today, ClientId = contractToRenew.DealMakerId };
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            var moneyBalance = await unitOfWork.MoneyBalanceRepository.GetTodayMoneyBalanceAsync();
            var dealDocument = new DealDocument { MoneyBalanceId = moneyBalance.TodayDate, Payment = payment, Cost = cost, Income = income, RepaymentCapital = repaymentCapital, Profit = profit };
            var renewContract = _mapper.Map<ContractRenew>(insertContractRenew);
            renewContract.DealDocument = dealDocument;
            var contractState = await _context.ContractStates.AsNoTracking().FirstOrDefaultAsync(c => c.State.Equals(RenewedContractState));
            Attach(contractToRenew);
            contractToRenew.ContractStateId = contractState.Id;
            contractToRenew.ContractRenews.Add(renewContract);
            await _context.SaveChangesAsync();
            await _context.Entry(renewContract).Reference(r => r.LendingRate).LoadAsync();
            return contractToRenew;
        }

        public async Task<Contract> BuyBackContract(Contract contractToBuyBack, PaymentType paymentType, decimal paymentAmount,
            decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default)
        {
            var payment = new Payment { PaymentTypeId = paymentType.Id, Amount = paymentAmount, Date = DateTime.Today, ClientId = contractToBuyBack.DealMakerId };
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            var moneyBalance = await unitOfWork.MoneyBalanceRepository.GetTodayMoneyBalanceAsync();
            var dealDocument = new DealDocument { MoneyBalanceId = moneyBalance.TodayDate, Payment = payment, Cost = cost, Income = income, RepaymentCapital = repaymentCapital, Profit = profit };
            var contractState = await _context.ContractStates.AsNoTracking().FirstOrDefaultAsync(c => c.State.Equals(BoughtBackContractState));
            Attach(contractToBuyBack);
            contractToBuyBack.ContractStateId = contractState.Id;
            contractToBuyBack.BuyBackId = contractToBuyBack.DealMakerId;
            contractToBuyBack.BuyBackDealDocument = dealDocument;
            await _context.SaveChangesAsync();

            return contractToBuyBack;
        }

        private IQueryable<Contract> GetTopContractsQueryable(int count)
        {
            return GetContractsQueryable()
                .Take(count);
        }

        private IQueryable<Contract> GetContractsQueryable()
        {
            return _context.Contracts
                .Include(p => p.ContractState)
                .Include(p => p.LendingRate)
                .Include(p => p.ContractItems)
                .ThenInclude(c => c.Category)
                .ThenInclude(c => c.Measure)
                .Include(p => p.DealMaker)
                .ThenInclude(p => p.ClientNavigation)
                .ThenInclude(person => person.Address)
                .ThenInclude(address => address.Country)
                .Include(c => c.DealMaker)
                .ThenInclude(c => c.ClientNavigation)
                .ThenInclude(c => c.Address)
                .ThenInclude(c => c.City)
                .Include(p => p.ContractRenews)
                .ThenInclude(c => c.LendingRate)
                .AsQueryable();
        }
    }
}