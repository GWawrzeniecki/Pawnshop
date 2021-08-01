using BespokeFusion;
using PawnShop.Business.Models;
using PawnShop.Core.Events;
using PawnShop.Core.ScopedRegion;
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
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class RenewContractPaymentViewModel : BindableBase, IRegionManagerAware, INavigationAware
    {

        #region PrivateMembers

        private PaymentType _selectedPaymentType;
        private IList<PaymentType> _paymentTypes;
        private readonly IContractService _contractService;
        private readonly IShellService _shellService;
        private readonly IEventAggregator _eventAggregator;
        private decimal _renewPrice;
        private Business.Models.Contract _contractToRenew;
        private DelegateCommand _saveCommand;
        private bool _isPrintDealDocument;
        private LendingRate _renewLendingRate;
        private DateTime _startDate;

        #endregion

        #region Constructor
        public RenewContractPaymentViewModel(IContractService contractService, IShellService shellService, IEventAggregator eventAggregator)
        {
            _contractService = contractService;
            _shellService = shellService;
            _eventAggregator = eventAggregator;
            LoadStartupData();
        }

        #endregion

        #region PublicProperties


        public PaymentType SelectedPaymentType
        {
            get => _selectedPaymentType;
            set => SetProperty(ref _selectedPaymentType, value);
        }


        public IList<PaymentType> PaymentTypes
        {
            get => _paymentTypes;
            set => SetProperty(ref _paymentTypes, value);
        }


        public decimal RenewPrice
        {
            get => _renewPrice;
            set => SetProperty(ref _renewPrice, value);
        }


        public bool IsPrintDealDocument
        {
            get => _isPrintDealDocument;
            set => SetProperty(ref _isPrintDealDocument, value);
        }

        #endregion

        #region Commands

        public DelegateCommand SaveCommand =>
            _saveCommand ??=
                new DelegateCommand(Save, CanExecuteMethodSave).ObservesProperty(() => SelectedPaymentType);

        #endregion Commands

        #region CommandMethods

        private bool CanExecuteMethodSave()
        {
            return SelectedPaymentType is not null;
        }

        private async void Save()
        {
            try
            {
                if (_shellService.GetShellViewModel<RenewContractWindow>() is RenewContractWindowViewModel vm)
                    vm.IsBusy = true;

                var insertContractRenew = new InsertContractRenew()
                {
                    ClientId = _contractToRenew.DealMakerId,
                    ContractNumberId = _contractToRenew.ContractNumberId,
                    LendingRateId = _renewLendingRate.Id,
                    StartDate = _startDate
                };

                await TryToRenewContract(_contractToRenew, insertContractRenew, SelectedPaymentType, RenewPrice, null,
                    RenewPrice);
                _eventAggregator.GetEvent<MoneyBalanceChangedEvent>().Publish();
                MaterialMessageBox.Show($"Pomyślnie przedłużono umowę.", "Sukces");
                if (IsPrintDealDocument)
                    await TryToPrintDealDocumentAsync();

            }
            catch (RenewContractException renewContractException)
            {
                MaterialMessageBox.ShowError(
                    $"{renewContractException.Message}{Environment.NewLine}Błąd: {renewContractException.InnerException?.Message}",
                    "Błąd");
            }
            catch (PrintDealDocumentException dealDocumentException)
            {
                MaterialMessageBox.ShowError(
                    $"{dealDocumentException.Message}{Environment.NewLine}Błąd: {dealDocumentException.InnerException?.Message}",
                    "Błąd");
            }
            finally
            {
                if (_shellService.GetShellViewModel<RenewContractWindow>() is RenewContractWindowViewModel vm)
                    vm.IsBusy = false;
                _shellService.CloseShell<RenewContractWindow>();
            }
        }



        #endregion

        #region PrivateMethods

        private async void LoadStartupData()
        {
            try
            {
                await TryToLoadPaymentTypes();

            }

            catch (LoadingPaymentTypesException loadingPaymentTypesException)
            {
                MaterialMessageBox.ShowError(
                    $"{loadingPaymentTypesException.Message}{Environment.NewLine}Błąd: {loadingPaymentTypesException.InnerException?.Message}",
                    "Błąd");
            }

        }

        private async Task TryToLoadPaymentTypes()
        {
            PaymentTypes = await _contractService.LoadPaymentTypes();
        }

        private async Task TryToRenewContract(Business.Models.Contract contractToRenew, InsertContractRenew insertContractRenew, PaymentType paymentType, decimal paymentAmount,
            decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default)
        {
            await _contractService.RenewContract(contractToRenew, insertContractRenew, paymentType, paymentAmount, cost, income, repaymentCapital, profit);
        }

        private async Task TryToPrintDealDocumentAsync()
        {
            await _contractService.PrintDealDocument(_contractToRenew);
        }

        #endregion

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion IRegionManagerAware

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            var contract = navigationContext.Parameters.GetValue<Business.Models.Contract>("contract");
            if (contract is not null)
                _contractToRenew = contract;
            var renewPrice = navigationContext.Parameters.GetValue<decimal>("renewPrice");
            if (renewPrice != default)
                RenewPrice = renewPrice;
            var renewLendingRate = navigationContext.Parameters.GetValue<LendingRate>("renewLendingRate");
            if (renewLendingRate is not null)
                _renewLendingRate = renewLendingRate;
            var startDate = navigationContext.Parameters.GetValue<DateTime>("startDate");
            if (startDate != default)
                _startDate = startDate;
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
