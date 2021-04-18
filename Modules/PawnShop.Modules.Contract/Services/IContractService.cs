using System.Collections.Generic;
using System.Threading.Tasks;
using PawnShop.Business.Models;
using PawnShop.Services.DataService.QueryDataModels;

namespace PawnShop.Modules.Contract.Services
{
    public interface IContractService
    {
        Task<IList<LendingRate>> LoadLendingRates();

        Task<IList<ContractState>> LoadContractStates();

        Task<IList<Business.Models.Contract>> LoadContracts();

        Task<IList<Business.Models.Contract>> GetContracts(ContractQueryData queryData, int count);

        Task<IList<Client>> GetClientBySurname(string surname);

        Task<IList<Client>> GetClientByPesel(string pesel);
        //Task<Client> CreateClient(Client client);
    }
}