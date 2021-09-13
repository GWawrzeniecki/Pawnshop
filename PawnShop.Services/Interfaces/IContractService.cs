using PawnShop.Business.Models;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.Services.DataService.InsertModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawnShop.Services.Interfaces
{
    public interface IContractService
    {
        Task<IList<LendingRate>> LoadLendingRates();

        Task<IList<ContractState>> LoadContractStates();
        Task<IList<PaymentType>> LoadPaymentTypes();

        Task<IList<Contract>> LoadContracts(int count);

        Task<IList<Contract>> LoadContracts(Client client, int count);

        Task<IList<Contract>> GetContracts(ContractQueryData queryData, int count);

        Task<string> GetNextContractNumber();
        Task<Contract> CreateContract(InsertContract insertContract, string paymentTypeStr, decimal paymentAmount,
            DateTime paymentDate, decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default);

        Task PrintDealDocument(Contract contract);

        Task<Contract> RenewContract(Contract contractToRenew, InsertContractRenew insertContractRenew, PaymentType paymentType, decimal paymentAmount,
            decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default);
        Task<Contract> BuyBackContract(Contract buyBackContract, PaymentType paymentType, decimal paymentAmount,
            decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default);
    }
}