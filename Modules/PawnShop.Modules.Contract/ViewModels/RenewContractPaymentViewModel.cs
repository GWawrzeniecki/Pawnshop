using PawnShop.Business.Models;
using PawnShop.Core.Events;
using PawnShop.Core.ScopedRegion;
using PawnShop.Core.Tasks;
using PawnShop.Exceptions;
using PawnShop.Exceptions.DBExceptions;
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
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class RenewContractPaymentViewModel : BindableBase, IRegionManagerAware, INavigationAware
    {

        #region PrivateMembers

        private PaymentType _selectedPaymentType;
        private NotifyTask<IList<PaymentType>> _paymentTypes;
        private readonly IContractService _contractService;
        private readonly IShellService _shellService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageBoxService _messageBoxService;
        private decimal _renewPrice;
        private Business.Models.Contract _contractToRenew;
        private DelegateCommand _renewContractCommand;
        private bool _isPrintDealDocument;
        private LendingRate _renewLendingRate;
        private DateTime _startDate;
        private Func<Task> _callBack;

        #endregion

        #region Constructor
        public RenewContractPaymentViewModel(IContractService contractService, IShellService shellService, IEventAggregator eventAggregator, IMessageBoxService messageBoxService)
        {
            _contractService = contractService;
            _shellService = shellService;
            _eventAggregator = eventAggregator;
            _messageBoxService = messageBoxService;
            PaymentTypes = NotifyTask.Create(LoadPaymentTypes);
        }

        #endregion

        #region PublicProperties


        public PaymentType SelectedPaymentType
        {
            get => _selectedPaymentType;
            set => SetProperty(ref _selectedPaymentType, value);
        }


        public NotifyTask<IList<PaymentType>> PaymentTypes
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

        public DelegateCommand RenewContractCommand =>
            _renewContractCommand ??=
                new DelegateCommand(RenewContract, CanExecuteRenewContract).ObservesProperty(() => SelectedPaymentType);

        #endregion Commands

        #region CommandMethods

        private bool CanExecuteRenewContract()
        {
            return SelectedPaymentType is not null;
        }

        private async void RenewContract()
        {
            try
            {
                if (_shellService.GetShellViewModel<RenewContractWindow>() is RenewContractWindowViewModel vm)
                    vm.IsBusy = true;

                var insertContractRenew = new InsertContractRenew
                {
                    ClientId = _contractToRenew.DealMakerId,
                    ContractNumberId = _contractToRenew.ContractNumberId,
                    LendingRateId = _renewLendingRate.Id,
                    StartDate = _startDate
                };

                await TryToRenewContract(_contractToRenew, insertContractRenew, SelectedPaymentType, RenewPrice, null,
                    RenewPrice);
                _eventAggregator.GetEvent<MoneyBalanceChangedEvent>().Publish();
                _messageBoxService.Show("Pomyślnie przedłużono umowę.", "Sukces");
                if (IsPrintDealDocument)
                    await TryToPrintDealDocumentAsync();
                if (_callBack is not null)
                    await _callBack.Invoke();

            }
            catch (RenewContractException renewContractException)
            {
                _messageBoxService.ShowError(
                    $"{renewContractException.Message}{Environment.NewLine}Błąd: {renewContractException.InnerException?.Message}",
                    "Błąd");
            }
            catch (PrintDealDocumentException dealDocumentException)
            {
                _messageBoxService.ShowError(
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

        private async Task<IList<PaymentType>> LoadPaymentTypes()
        {
            try
            {
                return await _contractService.LoadPaymentTypes();
            }
            catch (LoadingPaymentTypesException loadingPaymentTypesException)
            {
                _messageBoxService.ShowError(
                    $"{loadingPaymentTypesException.Message}{Environment.NewLine}Błąd: {loadingPaymentTypesException.InnerException?.Message}",
                    "Błąd");
            }

            return Enumerable.Empty<PaymentType>().ToList();
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
            _callBack = navigationContext.Parameters.GetValue<Func<Task>>("CallBack");
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
