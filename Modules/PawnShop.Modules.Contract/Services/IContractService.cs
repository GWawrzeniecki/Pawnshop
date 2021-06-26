using PawnShop.Business.Models;
using PawnShop.Services.DataService.QueryDataModels;
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
        Task<PawnShop.Business.Models.Contract> CreateContract(PawnShop.Business.Models.Contract contract);


    }
}