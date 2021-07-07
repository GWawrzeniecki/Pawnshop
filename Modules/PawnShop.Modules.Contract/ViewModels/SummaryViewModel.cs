using AutoMapper;
using BespokeFusion;
using PawnShop.Business.Models;
using PawnShop.Core.Constants;
using PawnShop.Core.Events;
using PawnShop.Core.ScopedRegion;
using PawnShop.Core.SharedVariables;
using PawnShop.Exceptions;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.Services;
using PawnShop.Modules.Contract.Windows.ViewModels;
using PawnShop.Modules.Contract.Windows.Views;
using PawnShop.Services.DataService.InsertModels;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class SummaryViewModel : BindableBase, IRegionManagerAware, INavigationAware
    {



        #region PrivateMembers
        private readonly ICalculateService _calculateService;
        private readonly IPdfService _pdfService;
        private readonly ISessionContext _sessionContext;
        private readonly IConfigData _configData;
        private readonly IContractService _contractService;
        private readonly IMapper _mapper;
        private readonly IShellService _shellService;
        private readonly IEventAggregator _eventAggregator;
        private Business.Models.Contract _contract;
        private bool _isPrintDealDocument;
        private DelegateCommand _createContractCommand;

        #endregion

        #region Constructor

        public SummaryViewModel(ICalculateService calculateService, IPdfService pdfService, ISessionContext sessionContext,
            IConfigData configData, IContractService contractService, IMapper mapper, IShellService shellService, IEventAggregator eventAggregator)
        {
            _calculateService = calculateService;
            _pdfService = pdfService;
            _sessionContext = sessionContext;
            _configData = configData;
            _contractService = contractService;
            _mapper = mapper;
            _shellService = shellService;
            _eventAggregator = eventAggregator;
            Contract = new Business.Models.Contract();
        }
        #endregion
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

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion IRegionManagerAware

        #region Commands


        public DelegateCommand CreateContractCommand =>
            _createContractCommand ??= new DelegateCommand(CreateContract);





        #endregion

        #region CommandMethods
        private async void CreateContract()
        {
            try
            {
                if (_shellService.GetShellViewModel<CreateContractWindow>() is CreateContractWindowViewModel vm)
                    vm.IsBusy = true;
                await AddContractToDbAsync();
                if (IsPrintDealDocument)
                    await PrintDealDocumentAsync();
                _eventAggregator.GetEvent<MoneyBalanceChangedEvent>().Publish();
                MaterialMessageBox.Show($"Pomyślnie utworzono umowę.", "Sukces");

            }
            catch (CreateContractException createContractException)
            {
                MaterialMessageBox.ShowError(
                    $"{createContractException.Message}{Environment.NewLine}Błąd: {createContractException.InnerException?.Message}",
                    "Błąd");
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
            finally
            {
                if (_shellService.GetShellViewModel<CreateContractWindow>() is CreateContractWindowViewModel vm)
                    vm.IsBusy = false;
                _shellService.CloseShell<CreateContractWindow>();
            }
        }




        #endregion


        #region PrivateMethods



        private async Task AddContractToDbAsync()
        {

            var insertContract = _mapper.Map<InsertContract>(Contract);

            await _contractService.CreateContract(insertContract, Constants.CashPaymentType, SumOfEstimatedValues, DateTime.Now, SumOfEstimatedValues);
        }

        private async Task PrintDealDocumentAsync()
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
                    fieldNameFieldValue.Add(($"Description{i}", Contract.ContractItems.ToArray()[i - 1].Name));
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
                await _pdfService.FillPdfFormAsync(_configData.DealDocumentPath, path, fieldNameFieldValue.ToArray());
                await _pdfService.PrintPdfAsync(path);

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
            Contract.LendingRateId = Contract.LendingRate.Id;
            Contract.DealMaker = navigationContext.Parameters.GetValue<Client>("DealMaker");
            Contract.DealMakerId = Contract.DealMaker.ClientId;
            Contract.StartDate = navigationContext.Parameters.GetValue<DateTime>("StartDate");
            Contract.ContractNumberId = navigationContext.Parameters.GetValue<string>("ContractNumber");
            Contract.AmountContract = SumOfEstimatedValues;
            Contract.WorkerBossId = _sessionContext.LoggedPerson.WorkerBossId;

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