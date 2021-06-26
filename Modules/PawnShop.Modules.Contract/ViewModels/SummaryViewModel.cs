﻿using BespokeFusion;
using PawnShop.Business.Models;
using PawnShop.Core.SharedVariables;
using PawnShop.Exceptions;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class SummaryViewModel : BindableBase, INavigationAware
    {



        #region PrivateMembers
        private readonly ICalculateService _calculateService;
        private readonly IPdfService _pdfService;
        private readonly ISessionContext _sessionContext;
        private readonly IConfigData _configData;
        private Business.Models.Contract _contract;
        private bool _isPrintDealDocument;
        private DelegateCommand _createContractCommand;

        #endregion

        #region Constructor

        public SummaryViewModel(ICalculateService calculateService, IPdfService pdfService, ISessionContext sessionContext, IConfigData configData)
        {
            _calculateService = calculateService;
            _pdfService = pdfService;
            _sessionContext = sessionContext;
            _configData = configData;
            Contract = new Business.Models.Contract();
        }

        #region PublicProperties


        public Business.Models.Contract Contract
        {
            get => _contract;
            set => SetProperty(ref _contract, value);
        }


        public bool IsPrintDealDocument
        {
            get => _isPrintDealDocument;
            set => SetProperty(ref _isPrintDealDocument, value);
        }

        public decimal SumOfEstimatedValues => Contract.ContractItems.Sum(c => c.EstimatedValue);

        public decimal RePurchasePrice =>
            _calculateService.CalculateContractAmount(SumOfEstimatedValues, Contract.LendingRate);

        public decimal NetStorageCost =>
            _calculateService.CalculateNetStorageCost(SumOfEstimatedValues, Contract.LendingRate);

        public decimal PCC => SumOfEstimatedValues >= 1000 ? SumOfEstimatedValues * 2 / 100 : 0;

        #endregion

        #endregion

        #region Commands


        public DelegateCommand CreateContractCommand =>
            _createContractCommand ??= new DelegateCommand(CreateContract);



        #endregion

        #region CommandMethods
        private void CreateContract()
        {
            try
            {
                TryToCreateContract();
                //close shell
            }
            catch (PrintDealDocumentException printDealDocumentException)
            {
                MaterialMessageBox.ShowError(
                    $"{printDealDocumentException.Message}{Environment.NewLine}Błąd: {printDealDocumentException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }




        #endregion


        #region PrivateMethods

        private void TryToCreateContract()
        {
            //Create contract in DB
            //print contract
            if (IsPrintDealDocument)
                TryToPrintDealDocument();

        }

        private void TryToPrintDealDocument()
        {
            try
            {
                var fieldNameFieldValue = new List<(string, string)>
                {
                    ("TodayDate", Contract.StartDate.ToShortDateString()),
                    ("ContractNumber", Contract.ContractNumberId),
                    ("FirstNameLastName", Contract.DealMaker.ClientNavigation.FullName),
                    ("Street", Contract.DealMaker.ClientNavigation.Address.Street),
                    ("City", Contract.DealMaker.ClientNavigation.Address.City.City1),
                    ("HouseNumber", Contract.DealMaker.ClientNavigation.Address.HouseNumber),
                    ("ApartmentNumber", Contract.DealMaker.ClientNavigation.Address.ApartmentNumber),
                    ("PostCode", Contract.DealMaker.ClientNavigation.Address.PostCode),
                    ("BirthDate", Contract.DealMaker.ClientNavigation.BirthDate.ToShortDateString()),
                    ("P1", Contract.DealMaker.Pesel[0].ToString()),
                    ("P2", Contract.DealMaker.Pesel[1].ToString()),
                    ("P3", Contract.DealMaker.Pesel[2].ToString()),
                    ("P4", Contract.DealMaker.Pesel[3].ToString()),
                    ("P5", Contract.DealMaker.Pesel[4].ToString()),
                    ("P6", Contract.DealMaker.Pesel[5].ToString()),
                    ("P7", Contract.DealMaker.Pesel[6].ToString()),
                    ("P8", Contract.DealMaker.Pesel[7].ToString()),
                    ("P9", Contract.DealMaker.Pesel[8].ToString()),
                    ("P10", Contract.DealMaker.Pesel[9].ToString()),
                    ("P111", Contract.DealMaker.Pesel[10].ToString()),
                    ("IDCardNumber1", Contract.DealMaker.IdcardNumber[..2]),
                    ("IDCardNumber2", Contract.DealMaker.IdcardNumber[3..])
                };



                for (var i = 1; i <= Contract.ContractItems.Count; i++)
                {
                    fieldNameFieldValue.Add(($"LpRow{i}", i.ToString()));
                    fieldNameFieldValue.Add(($"Description{i}", Contract.ContractItems.ToArray()[i - 1].Description));
                    fieldNameFieldValue.Add(($"JmRow{i}", Contract.ContractItems.ToArray()[i - 1].Category.Measure.Measure));
                    fieldNameFieldValue.Add(($"Quantity{i}", Contract.ContractItems.ToArray()[i - 1].Amount.ToString()));
                    fieldNameFieldValue.Add(($"EstimatedValue{i}", Contract.ContractItems.ToArray()[i - 1].EstimatedValue.ToString()));
                    fieldNameFieldValue.Add(($"Condition{i}", Contract.ContractItems.ToArray()[i - 1].TechnicalCondition));
                }

                fieldNameFieldValue.Add(("EstimatedValueSum", SumOfEstimatedValues.ToString()));
                fieldNameFieldValue.Add(("PCC", PCC.ToString()));
                fieldNameFieldValue.Add(("RePurchaseDate", Contract.StartDate.AddDays(Contract.LendingRate.Days).ToShortDateString()));
                fieldNameFieldValue.Add(("RePurchasePrice", RePurchasePrice.ToString()));
                fieldNameFieldValue.Add(("NetStorageCost", NetStorageCost.ToString()));

                var path = $@"{_configData.DealDocumentsFolderPath}\{Contract.ContractNumberId.Replace('/', '.')}.pdf";
                _pdfService.FillPdfForm(_configData.DealDocumentPath, path, fieldNameFieldValue.ToArray());
                _pdfService.PrintPdf(path);

            }
            catch (Exception e)
            {
                throw new PrintDealDocumentException("Wystąpił problem podczas drukowania umowy.", e);
            }
        }

        #endregion

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            Contract.ContractItems = new Collection<ContractItem>(navigationContext.Parameters.GetValue<IList<ContractItem>>("ContractItems"));
            Contract.LendingRate = navigationContext.Parameters.GetValue<LendingRate>("LendingRate");
            Contract.DealMaker = navigationContext.Parameters.GetValue<Client>("DealMaker");
            Contract.StartDate = navigationContext.Parameters.GetValue<DateTime>("StartDate");
            Contract.ContractNumberId = navigationContext.Parameters.GetValue<string>("ContractNumber");
            Contract.AmountContract = SumOfEstimatedValues;
            Contract.WorkerBoss = _sessionContext.LoggedPerson;

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        #endregion

    }
}