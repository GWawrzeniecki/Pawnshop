using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PawnShop.Business.Models;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.DataService;
using PawnShop.Services.DataService.QueryDataModels;

namespace PawnShop.Modules.Contract.Services
{
    public class ContractService : IContractService
    {
        #region private members

        private readonly IUnitOfWork _unitOfWork;

        #endregion private members

        #region constructor

        public ContractService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion constructor

        #region IContractService interface

        public async Task<IList<ContractState>> LoadContractStates()
        {
            try
            {
                return await TryToLoadContractStates();
            }
            catch (Exception e)
            {
                throw new LoadingContractStatesException("Wystąpił problem podczas ładowania rodzajów stanów umowy.",
                    e);
            }
        }

        public async Task<IList<LendingRate>> LoadLendingRates()
        {
            try
            {
                return await TryToLoadLendingRate();
            }
            catch (Exception e)
            {
                throw new LoadingLendingRatesException(
                    "Wystąpił problem podczas ładowania rodzajów czasu trwania umowy.", e);
            }
        }

        public async Task<IList<Business.Models.Contract>> LoadContracts()
        {
            try
            {
                return await TryToLoadContracts();
            }
            catch (Exception e)
            {
                throw new LoadingContractsException("Wystąpił problem podczas ładowania umów.", e);
            }
        }

        public async Task<IList<Business.Models.Contract>> GetContracts(ContractQueryData queryData, int count)
        {
            try
            {
                return await TryToGetContracts(queryData, count);
            }
            catch (Exception e)
            {
                throw new LoadingContractsException("Wystąpił problem podczas wyszukiwania umów.", e);
            }
        }

        public async Task<IList<Client>> GetClientBySurname(string surname)
        {
            try
            {
                return await TryToGetClientBySurname(surname);
            }
            catch (Exception e)
            {
                throw new SearchClientsException("Wystąpił problem podczas wyszukiwania klientów po nazwisku.", e);
            }
        }

        public async Task<IList<Client>> GetClientByPesel(string pesel)
        {
            try
            {
                return await TryToGetClientByPesel(pesel);
            }
            catch (Exception e)
            {
                throw new SearchClientsException("Wystąpił problem podczas wyszukiwania klientów po numerze pesel.", e);
            }
        }

        //public Task<Client> CreateClient(Client client)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion IContractService interface

        #region private methods

        private async Task<IList<ContractState>> TryToLoadContractStates()
        {
            return (await _unitOfWork.ContractStateRepository.GetAsync()).ToList();
        }

        private async Task<IList<LendingRate>> TryToLoadLendingRate()
        {
            return (await _unitOfWork.LendingRateRepository.GetAsync()).ToList();
        }

        private async Task<IList<Business.Models.Contract>> TryToLoadContracts()
        {
            return await _unitOfWork.ContractRepository.GetTopContractsAsync(100);
        }

        public async Task<IList<Business.Models.Contract>> TryToGetContracts(ContractQueryData queryData, int count)
        {
            return await _unitOfWork.ContractRepository.GetContracts(queryData, count);
        }

        private async Task<IList<Client>> TryToGetClientBySurname(string surname)
        {
            return await _unitOfWork.ClientRepository.GetClientBySurname(surname);
        }

        private async Task<IList<Client>> TryToGetClientByPesel(string pesel)
        {
            return await _unitOfWork.ClientRepository.GetClientByPesel(pesel);
        }

        //private async Task<Client> TryToCreateClient(Client client)
        //{
        //    return await _unitOfWork.ClientRepository.GetClientByPesel(pesel);
        //}


        #endregion private methods
    }
}