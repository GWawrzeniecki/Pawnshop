﻿using PawnShop.Business.Models;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.DataService;
using PawnShop.Services.DataService.InsertModels;
using PawnShop.Services.DataService.QueryDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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


        public async Task<string> GetNextContractNumber()
        {
            try
            {
                return await TryToGetNextContractNumber();
            }
            catch (Exception e)
            {
                throw new GetNextContractNumberException("Wystąpił problem podczas pobierania kolejnego numeru umowy", e);
            }
        }

        public async Task<Business.Models.Contract> CreateContract(InsertContract insertContract, string paymentTypeStr, decimal paymentAmount,
            DateTime paymentDate, decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default)
        {
            try
            {
                return await TryToCreateContract(insertContract, paymentTypeStr, paymentAmount, paymentDate, cost, income, repaymentCapital, profit);
            }
            catch (Exception e)
            {
                throw new CreateContractException("Wystąpił problem podczas tworzenia umowy.", e);
            }
        }



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

        public async Task<string> TryToGetNextContractNumber()
        {
            return await _unitOfWork.ContractRepository.GetNextContractNumber();
        }

        private async Task<Business.Models.Contract> TryToCreateContract(InsertContract insertContract, string paymentTypeStr, decimal paymentAmount,
            DateTime paymentDate, decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default)
        {
            return await _unitOfWork.ContractRepository.CreateContract(insertContract, paymentTypeStr, paymentAmount, paymentDate, cost, income, repaymentCapital, profit);

        }

        #endregion private methods
    }
}