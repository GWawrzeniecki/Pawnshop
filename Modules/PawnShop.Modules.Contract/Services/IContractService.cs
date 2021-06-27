using PawnShop.Business.Models;
using PawnShop.Services.DataService.InsertModels;
using PawnShop.Services.DataService.QueryDataModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.Services
{
    public interface IContractService
    {
        Task<IList<LendingRate>> LoadLendingRates();

        Task<IList<ContractState>> LoadContractStates();

        Task<IList<Business.Models.Contract>> LoadContracts();

        Task<IList<Business.Models.Contract>> GetContracts(ContractQueryData queryData, int count);

        Task<string> GetNextContractNumber();
        Task<Business.Models.Contract> CreateContract(InsertContract insertContract, string paymentTypeStr, decimal paymentAmount,
            DateTime paymentDate, decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default);


    }
}