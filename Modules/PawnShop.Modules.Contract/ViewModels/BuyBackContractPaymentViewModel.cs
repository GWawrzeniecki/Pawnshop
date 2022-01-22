using PawnShop.Business.Models;
using PawnShop.Core.Events;
using PawnShop.Core.ScopedRegion;
using PawnShop.Exceptions;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.Views;
using PawnShop.Modules.Contract.Windows.ViewModels;
using PawnShop.Modules.Contract.Windows.Views;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class BuyBackContractPaymentViewModel : BindableBase, IRegionManagerAware, INavigationAware
    {
        #region PrivateMembers

        private readonly IContractService _contractService;
        private readonly IShellService _shellService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IContainerProvider _containerProvider;
        private readonly IPrintService _printService;
        private readonly IMessageBoxService _messageBoxService;
        private decimal _buyBackPrice;
        private Business.Models.Contract _contractToBuyBack;
        private IList<PaymentType> _paymentTypes;
        private PaymentType _selectedPaymentType;
        private bool _isPrintDealDocument;
        private bool _isPrintContractItems;
        private DelegateCommand _saveCommand;
        private Func<Task> _callBack;

        #endregion

        #region Constructor

        public BuyBackContractPaymentViewModel(IContractService contractService, IShellService shellService, IEventAggregator eventAggregator, IContainerProvider containerProvider, IPrintService printService, IMessageBoxService messageBoxService)
        {
            _contractService = contractService;
            _shellService = shellService;
            _eventAggregator = eventAggregator;
            _containerProvider = containerProvider;
            _printService = printService;
            _messageBoxService = messageBoxService;
            LoadStartupData();
        }

        #endregion

        #region PublicProperties

        public decimal BuyBackPrice
        {
            get => _buyBackPrice;
            set => SetProperty(ref _buyBackPrice, value);
        }

        public IList<PaymentType> PaymentTypes
        {
            get => _paymentTypes;
            set => SetProperty(ref _paymentTypes, value);
        }

        public PaymentType SelectedPaymentType
        {
            get => _selectedPaymentType;
            set => SetProperty(ref _selectedPaymentType, value);
        }

        public bool IsPrintDealDocument
        {
            get => _isPrintDealDocument;
            set => SetProperty(ref _isPrintDealDocument, value);
        }

        public bool IsPrintContractItems
        {
            get => _isPrintContractItems;
            set => SetProperty(ref _isPrintContractItems, value);
        }

        #endregion

        #region PrivateProperties

        public decimal SumOfEstimatedValues => _contractToBuyBack.ContractItems.Sum(c => c.EstimatedValue);

        #endregion

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var contract = navigationContext.Parameters.GetValue<Business.Models.Contract>("contract");
            if (contract is not null)
                _contractToBuyBack = contract;
            var buyBackPrice = navigationContext.Parameters.GetValue<decimal>("buyBackPrice");
            if (buyBackPrice != default)
                BuyBackPrice = buyBackPrice;
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

        #region Commands

        public DelegateCommand BuyBackContractCommand =>
            _saveCommand ??=
                new DelegateCommand(BuyBackContract, CanExecuteBuyBackContract).ObservesProperty(() => SelectedPaymentType);

        #endregion Commands

        #region CommandMethods

        private bool CanExecuteBuyBackContract()
        {
            return SelectedPaymentType is not null;
        }

        private async void BuyBackContract()
        {
            try
            {
                if (_shellService.GetShellViewModel<BuyBackContractWindow>() is BuyBackContractWindowViewModel vm)
                    vm.IsBusy = true;

                await TryToBuybackContract(_contractToBuyBack, SelectedPaymentType, BuyBackPrice, null, null,
                    BuyBackPrice, BuyBackPrice - SumOfEstimatedValues);

                _eventAggregator.GetEvent<MoneyBalanceChangedEvent>().Publish();
                _messageBoxService.Show("Pomyślnie wykupiono umowę.", "Sukces");

                if (IsPrintDealDocument)
                    await TryToPrintDealDocumentAsync();
                if (IsPrintContractItems)
                    await TryToPrintContractItemsAsync();
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
            catch (BuyBackContractException buyBackContractException)
            {
                _messageBoxService.ShowError(
                    $"{buyBackContractException.Message}{Environment.NewLine}Błąd: {buyBackContractException.InnerException?.Message}",
                    "Błąd");
            }
            catch (PrintVisualElementException printVisualElementException)
            {
                _messageBoxService.ShowError(
                    $"{printVisualElementException.Message}{Environment.NewLine}Błąd: {printVisualElementException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
            finally
            {
                if (_shellService.GetShellViewModel<BuyBackContractWindow>() is BuyBackContractWindowViewModel vm)
                    vm.IsBusy = false;
                _shellService.CloseShell<BuyBackContractWindow>();
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
                _messageBoxService.ShowError(
                    $"{loadingPaymentTypesException.Message}{Environment.NewLine}Błąd: {loadingPaymentTypesException.InnerException?.Message}",
                    "Błąd");
            }
        }

        private async Task TryToLoadPaymentTypes()
        {
            PaymentTypes = await _contractService.LoadPaymentTypes();
        }

        private async Task TryToBuybackContract(Business.Models.Contract buybackContract, PaymentType paymentType, decimal paymentAmount,
            decimal? cost, decimal? income = default, decimal? repaymentCapital = default, decimal? profit = default)
        {
            await _contractService.BuyBackContract(buybackContract, paymentType, paymentAmount, cost, income, repaymentCapital, profit);
        }

        private async Task TryToPrintDealDocumentAsync()
        {
            await _contractService.PrintDealDocument(_contractToBuyBack);
        }

        private async Task TryToPrintContractItemsAsync()
        {
            try
            {

                var buyBackContractItems = _containerProvider.Resolve<BuyBackContractItems>();
                (buyBackContractItems.DataContext as BuyBackContractItemsViewModel).ContractToBuyBack =
                    _contractToBuyBack;
                await Task.Factory.StartNew(
                      () => _printService.PrintVisualElement(buyBackContractItems.ContractItemsDataGrid),
                      CancellationToken.None, TaskCreationOptions.None,
                      TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception e)
            {
                throw new PrintVisualElementException("Wystąpił błąd podczas drukowania towarów do wyjęcia.", e);
            }
        }

        #endregion
    }
}