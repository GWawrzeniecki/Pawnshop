﻿using PawnShop.Business.Models;
using PawnShop.Core.SharedVariables;
using PawnShop.Exceptions;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.DataService;
using PawnShop.Services.DataService.InsertModels;
using PawnShop.Services.DataService.QueryDataModels;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.Services
{
    public class ContractService : IContractService
    {
        #region private members

        private readonly IPdfService _pdfService;
        private readonly IUserSettings _userSettings;
        private readonly ICalculateService _calculateService;
        private readonly IContainerProvider _containerProvider;

        #endregion private members

        #region constructor

        public ContractService(IPdfService pdfService, IUserSettings userSettings,
            ICalculateService calculateService, IContainerProvider containerProvider)
        {
            _pdfService = pdfService;
            _userSettings = userSettings;
            _calculateService = calculateService;
            _containerProvider = containerProvider;
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

        public async Task<IList<PaymentType>> LoadPaymentTypes()
        {
            try
            {
                return await TryToTLoadPaymentTypes();
            }
            catch (Exception e)
            {
                throw new LoadingPaymentTypesException(
                    "Wystąpił problem podczas ładowania typów płatności", e);
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
                throw new GetNextContractNumberException("Wystąpił problem podczas pobierania kolejnego numeru umowy",
                    e);
            }
        }

        public async Task<Business.Models.Contract> CreateContract(InsertContract insertContract, string paymentTypeStr,
            decimal paymentAmount,
            DateTime paymentDate, decimal? cost, decimal? income = default, decimal? repaymentCapital = default,
            decimal? profit = default)
        {
            try
            {
                return await TryToCreateContract(insertContract, paymentTypeStr, paymentAmount, paymentDate, cost,
                    income, repaymentCapital, profit);
            }
            catch (Exception e)
            {
                throw new CreateContractException("Wystąpił problem podczas tworzenia umowy.", e);
            }
        }

        public async Task PrintDealDocument(Business.Models.Contract contract)
        {
            try
            {
                await TryToPrintDealDocument(contract);
            }
            catch (Exception e)
            {
                throw new PrintDealDocumentException("Wystąpił problem podczas drukowania umowy.", e);
            }
        }

        public async Task<Business.Models.Contract> RenewContract(Business.Models.Contract contractToRenew,
            InsertContractRenew insertContractRenew, PaymentType paymentType, decimal paymentAmount,
            decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default)
        {
            try
            {
                return await TryToRenewContract(contractToRenew, insertContractRenew, paymentType, paymentAmount, cost,
                    income, repaymentCapital, profit);
            }
            catch (Exception e)
            {
                throw new RenewContractException("Wystąpił problem podczas przedłużania umowy.", e);
            }
        }

        public async Task<Business.Models.Contract> BuyBackContract(Business.Models.Contract buyBackContract,
            PaymentType paymentType, decimal paymentAmount, decimal? cost,
            decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default)
        {
            try
            {
                return await TryToBuyBackContract(buyBackContract, paymentType, paymentAmount, cost, income,
                    repaymentCapital, profit);
            }
            catch (Exception e)
            {
                throw new BuyBackContractException("Wystąpił problem podczas wykupywania umowy.", e);
            }
        }

        #endregion IContractService interface

        #region private methods

        private async Task<IList<ContractState>> TryToLoadContractStates()
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return (await unitOfWork.ContractStateRepository.GetAsync()).ToList();
        }

        private async Task<IList<LendingRate>> TryToLoadLendingRate()
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return (await unitOfWork.LendingRateRepository.GetAsync()).ToList();
        }

        private async Task<IList<PaymentType>> TryToTLoadPaymentTypes()
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return (await unitOfWork.PaymentTypeRepository.GetAsync()).ToList();
        }

        private async Task<IList<Business.Models.Contract>> TryToLoadContracts()
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return await unitOfWork.ContractRepository.GetTopContractsAsync(100);
        }

        public async Task<IList<Business.Models.Contract>> TryToGetContracts(ContractQueryData queryData, int count)
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return await unitOfWork.ContractRepository.GetContracts(queryData, count);
        }

        public async Task<string> TryToGetNextContractNumber()
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return await unitOfWork.ContractRepository.GetNextContractNumber();
        }

        private async Task<Business.Models.Contract> TryToCreateContract(InsertContract insertContract,
            string paymentTypeStr, decimal paymentAmount,
            DateTime paymentDate, decimal? cost, decimal? income = default, decimal? repaymentCapital = default,
            decimal? profit = default)
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return await unitOfWork.ContractRepository.CreateContract(insertContract, paymentTypeStr, paymentAmount,
                paymentDate, cost, income, repaymentCapital, profit);
        }

        private async Task<Business.Models.Contract> TryToRenewContract(Business.Models.Contract contractToRenew,
            InsertContractRenew insertContractRenew, PaymentType paymentType, decimal paymentAmount,
            decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default)
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return await unitOfWork.ContractRepository.RenewContract(contractToRenew, insertContractRenew, paymentType,
                paymentAmount, cost, income, repaymentCapital, profit);
        }

        private async Task<Business.Models.Contract> TryToBuyBackContract(
            Business.Models.Contract buyBackContract, PaymentType paymentType, decimal paymentAmount, decimal? cost,
            decimal? income, decimal? repaymentCapital, decimal? profit)
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();

            return await unitOfWork.ContractRepository.BuyBackContract(buyBackContract, paymentType,
                paymentAmount, cost, income, repaymentCapital, profit);
        }

        private async Task TryToPrintDealDocument(Business.Models.Contract contract)
        {
            decimal SumOfEstimatedValues()
            {
                return contract.ContractItems.Sum(c => c.EstimatedValue);
            }

            decimal PCC()
            {
                return SumOfEstimatedValues() >= 1000 ? SumOfEstimatedValues() * 2 / 100 : 0;
            }

            decimal RePurchasePrice()
            {
                return _calculateService.CalculateContractAmount(SumOfEstimatedValues(), contract.LendingRate);
            }

            decimal NetStorageCost()
            {
                return _calculateService.CalculateNetStorageCost(SumOfEstimatedValues(), contract.LendingRate);
            }

            var fieldNameFieldValue = new List<(string, string)>
            {
                ("TodayDate", contract.StartDate.ToShortDateString()),
                ("ContractNumber", contract.ContractNumberId),
                ("FirstNameLastName", contract.DealMaker.ClientNavigation.FullName),
                ("Street", contract.DealMaker.ClientNavigation.Address.Street),
                ("City", contract.DealMaker.ClientNavigation.Address.City.City1),
                ("HouseNumber", contract.DealMaker.ClientNavigation.Address.HouseNumber),
                ("ApartmentNumber", contract.DealMaker.ClientNavigation.Address.ApartmentNumber),
                ("PostCode", contract.DealMaker.ClientNavigation.Address.PostCode),
                ("BirthDate", contract.DealMaker.ClientNavigation.BirthDate.ToShortDateString()),
                ("P1", contract.DealMaker.Pesel[0].ToString()),
                ("P2", contract.DealMaker.Pesel[1].ToString()),
                ("P3", contract.DealMaker.Pesel[2].ToString()),
                ("P4", contract.DealMaker.Pesel[3].ToString()),
                ("P5", contract.DealMaker.Pesel[4].ToString()),
                ("P6", contract.DealMaker.Pesel[5].ToString()),
                ("P7", contract.DealMaker.Pesel[6].ToString()),
                ("P8", contract.DealMaker.Pesel[7].ToString()),
                ("P9", contract.DealMaker.Pesel[8].ToString()),
                ("P10", contract.DealMaker.Pesel[9].ToString()),
                ("P111", contract.DealMaker.Pesel[10].ToString()),
                ("IDCardNumber1", contract.DealMaker.IdcardNumber[..2]),
                ("IDCardNumber2", contract.DealMaker.IdcardNumber[3..])
            };

            for (var i = 1; i <= contract.ContractItems.Count; i++)
            {
                fieldNameFieldValue.Add(($"LpContractItemRow{i}", i.ToString()));
                fieldNameFieldValue.Add(($"Description{i}", contract.ContractItems.ToArray()[i - 1].Name));
                fieldNameFieldValue.Add(($"JmRow{i}",
                    contract.ContractItems.ToArray()[i - 1].Category.Measure.Measure));
                fieldNameFieldValue.Add(($"Quantity{i}", contract.ContractItems.ToArray()[i - 1].Amount.ToString()));
                fieldNameFieldValue.Add(($"EstimatedValue{i}",
                    contract.ContractItems.ToArray()[i - 1].EstimatedValue.ToString()));
                fieldNameFieldValue.Add(($"Condition{i}", contract.ContractItems.ToArray()[i - 1].TechnicalCondition));
            }

            fieldNameFieldValue.Add(("EstimatedValueSum", SumOfEstimatedValues().ToString()));
            fieldNameFieldValue.Add(("PCC", PCC().ToString()));
            fieldNameFieldValue.Add(("RePurchaseDate",
                contract.StartDate.AddDays(contract.LendingRate.Days).ToShortDateString()));
            fieldNameFieldValue.Add(("RePurchasePrice", RePurchasePrice().ToString()));
            fieldNameFieldValue.Add(("NetStorageCost", NetStorageCost().ToString()));

            for (var i = 1; i <= contract.ContractRenews.Count; i++)
            {
                fieldNameFieldValue.Add(($"LpRenewRow{i}", i.ToString()));
                fieldNameFieldValue.Add(($"NewDateRePurchase{i}",
                    contract.ContractRenews.ToArray()[i - 1].StartDate
                        .AddDays(contract.ContractRenews.ToArray()[i - 1].LendingRate.Days).ToShortDateString()));
            }

            var path = $@"{_userSettings.DealDocumentsFolderPath}\{contract.ContractNumberId.Replace('/', '.')}.pdf";
            await _pdfService.FillPdfFormAsync(_userSettings.DealDocumentPath, path, fieldNameFieldValue.ToArray());
            await _pdfService.PrintPdfAsync(path);
        }

        #endregion private methods
    }
}