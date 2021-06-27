using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.Core.Extensions;
using PawnShop.DataAccess.Data;
using PawnShop.Services.DataService.InsertModels;
using PawnShop.Services.DataService.QueryDataModels;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _updateContractStatesProcedureName = "UpdateContractStates";

        public ContractRepository(PawnshopContext context, IUnitOfWork unitOfWork, IMapper mapper) : base(context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task UpdateContractStates() => await _context.Database.ExecuteSqlRawAsync($"Exec [{DBSchemaName}].[{_updateContractStatesProcedureName}]");

        public async Task<string> GetNextContractNumber()
        {
            var actualContractNumber = await _context
                .Contracts
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

        public async Task<IList<Contract>> GetContracts(ContractQueryData queryData, int count)
        {
            var contractQuery = _context.Contracts
                 .Include(p => p.ContractState)
                 .Include(p => p.LendingRate)
                 .Include(p => p.DealMaker)
                 .ThenInclude(p => p.ClientNavigation)
                 .AsQueryable();

            if (!string.IsNullOrEmpty(queryData.Client))
            {
                contractQuery = contractQuery.Where(p =>
                     (p.DealMaker.ClientNavigation.FirstName + " " + p.DealMaker.ClientNavigation.LastName).Contains(
                         queryData.Client));
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

            return await contractQuery
                .Take(count)
                .ToListAsync();
        }



        public async Task<Contract> CreateContract(InsertContract insertContract, string paymentTypeStr, decimal paymentAmount,
           DateTime paymentDate, decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default)
        {
            var contract = _mapper.Map<Contract>(insertContract);
            var paymentType = await _context.PaymentTypes.FirstOrDefaultAsync(p => p.Type.Equals(paymentTypeStr));
            var contractState =
                await _context.ContractStates.FirstOrDefaultAsync(c => c.State.Equals(CreatedContractState));
            var payment = new Payment { PaymentTypeId = paymentType.Id, Amount = paymentAmount, Date = paymentDate };
            var moneyBalance = await _unitOfWork.MoneyBalanceRepository.GetTodayMoneyBalanceAsync();
            var dealDocument = new DealDocument { MoneyBalanceId = moneyBalance.TodayDate, Payment = payment, Cost = cost, Income = income, RepaymentCapital = repaymentCapital, Profit = profit };
            contract.ContractStateId = contractState.Id;
            contract.DealDocument = dealDocument;
            await _context.Contracts.AddAsync(contract);
            await _context.SaveChangesAsync();

            return contract;
        }


    }
}