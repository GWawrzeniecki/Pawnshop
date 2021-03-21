using PawnShop.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PawnShop.Services.DataService.QueryDataModels;

namespace PawnShop.Modules.Contract.Services
{
    public interface IContractService
    {
        Task<IList<LendingRate>> LoadLendingRates();
        Task<IList<ContractState>> LoadContractStates();
        Task<IList<Business.Models.Contract>> LoadContracts();
        Task<IList<Business.Models.Contract>> GetContracts(ContractQueryData queryData, int count);
    }
}
