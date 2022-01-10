using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Core.Events;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Sale.Validators;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Sale.Dialogs.ViewModels
{
    public class SellDialogViewModel : ViewModelBase<SellDialogViewModel>, IDialogAware
    {
        #region PrivateMembers

        private string _title;
        private Business.Models.Sale _sale;
        private readonly IMapper _mapper;
        private readonly IContractService _contractService;
        private readonly IContainerProvider _containerProvider;
        private readonly IMessageBoxService _messageBoxService;
        private readonly IEventAggregator _eventAggregator;
        private string _itemName;
        private IList<UnitMeasure> _contractItemUnitMeasures;
        private UnitMeasure _selectedContractItemUnitMeasure;
        private decimal _boughtPrice;
        private decimal _sellPrice;
        private string _rack;
        private int? _shelf;
        private IList<Link> _saleLinks;
        private Link _selectedSaleLink;
        private string _saleLinkText;
        private UnitMeasure _selectedContractItemUnitMeasureForSale;
        private decimal _itemQuantity;
        private int _sellItemAmount;
        private decimal _soldPrice;
        private IList<PaymentType> _paymentTypes;
        private PaymentType _selectedPaymentType;
        private DelegateCommand _sellCommand;
        private DelegateCommand _cancelCommand;

        #endregion

        #region Constructor

        public SellDialogViewModel(IMapper mapper, IContractService contractService, IContainerProvider containerProvider, SellDialogValidator sellDialogValidator, IMessageBoxService messageBoxService, IEventAggregator eventAggregator) : base(sellDialogValidator)
        {
            _mapper = mapper;
            _contractService = contractService;
            _containerProvider = containerProvider;
            _messageBoxService = messageBoxService;
            _eventAggregator = eventAggregator;
            LoadStartupData();
        }

        #endregion

        #region PublicProperties

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public Business.Models.Sale Sale
        {
            get => _sale;
            set => SetProperty(ref _sale, value);
        }

        public string ItemName
        {
            get => _itemName;
            set => SetProperty(ref _itemName, value);
        }

        public IList<UnitMeasure> ContractItemUnitMeasures
        {
            get => _contractItemUnitMeasures;
            set => SetProperty(ref _contractItemUnitMeasures, value);
        }

        public UnitMeasure SelectedContractItemUnitMeasure
        {
            get => _selectedContractItemUnitMeasure;
            set => SetProperty(ref _selectedContractItemUnitMeasure, value);
        }

        public decimal BoughtPrice
        {
            get => _boughtPrice;
            set => SetProperty(ref _boughtPrice, value);
        }

        public decimal SellPrice
        {
            get => _sellPrice;
            set => SetProperty(ref _sellPrice, value);
        }

        public string Rack
        {
            get => _rack;
            set => SetProperty(ref _rack, value);
        }

        public int? Shelf
        {
            get => _shelf;
            set => SetProperty(ref _shelf, value);
        }

        public IList<Link> SaleLinks
        {
            get => _saleLinks;
            set => SetProperty(ref _saleLinks, value);
        }

        public Link SelectedSaleLink
        {
            get => _selectedSaleLink;
            set => SetProperty(ref _selectedSaleLink, value);
        }

        public string SaleLinkText
        {
            get => _saleLinkText;
            set => SetProperty(ref _saleLinkText, value);
        }

        public UnitMeasure SelectedContractItemUnitMeasureForSale
        {
            get => _selectedContractItemUnitMeasureForSale;
            set => SetProperty(ref _selectedContractItemUnitMeasureForSale, value);
        }

        public decimal ItemQuantity
        {
            get => _itemQuantity;
            set => SetProperty(ref _itemQuantity, value);
        }

        public int SellItemAmount
        {
            get => _sellItemAmount;
            set => SetProperty(ref _sellItemAmount, value);
        }

        public decimal SoldPrice
        {
            get => _soldPrice;
            set
            {
                SetProperty(ref _soldPrice, value);
                RaisePropertyChanged(nameof(Profit));
            }
        }

        public decimal Profit
        {
            get
            {
                if (ItemQuantity == 0)
                    return 0;

                return SoldPrice - (BoughtPrice / ItemQuantity);
            }
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


        #endregion

        #region Commands

        public DelegateCommand SellCommand => _sellCommand ??= new DelegateCommand(Sell, CanExecuteSell)
            .ObservesProperty(() => HasErrors);

        public DelegateCommand CancelCommand => _cancelCommand ??= new DelegateCommand(Cancel);

        #endregion

        #region CommandsMethods

        private bool CanExecuteSell() => !HasErrors;

        private async void Sell()
        {
            try
            {
                await TryToSellAsync();
                _messageBoxService.Show("Produkt sprzedany pomyślnie", "Sukces");
                _eventAggregator.GetEvent<MoneyBalanceChangedEvent>().Publish();
                RequestClose.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (SellItemException sellItemException)
            {
                _messageBoxService.ShowError(
                    $"{sellItemException.Message}{Environment.NewLine}Błąd: {sellItemException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private void Cancel()
        {
            RequestClose.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        #endregion

        #region viewModelBase

        protected override SellDialogViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase

        #region IDialogAware

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title");
            Sale = parameters.GetValue<Business.Models.Sale>("sale");
            MapSaleToVm();
        }


        public event Action<IDialogResult> RequestClose;

        #endregion IDialogAware

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

        private void MapSaleToVm()
        {
            _mapper.Map(Sale, this);
        }

        private async Task TryToSellAsync()
        {
            try
            {

                using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
                var profit = CalculateProfit();
                await unitOfWork.SaleRepository.Sell(_sale, SoldPrice, SelectedPaymentType, _containerProvider, profit);
            }
            catch (Exception e)
            {
                throw new SellItemException("Wystąpił błąd podczas sprzedaży produktu.", e);
            }
        }

        private decimal CalculateProfit()
        {
            if (Sale.ContractItem.Amount == 1)
                return Profit;


            var soldCount = Sale.ContractItem.Sales.Count(s => s.IsSold);

            if (soldCount == 0)
            {
                return (SoldPrice > BoughtPrice) ? SoldPrice - BoughtPrice : 0;
            }

            var soldItemsPrice = Sale.ContractItem.Sales
                .Where(s => s.IsSold)
                .Sum(s => s.SoldPrice);

            if (!soldItemsPrice.HasValue)
                throw new SellItemException("Wystąpił błąd w liczeniu zysku ze sprzedaży.");

            soldItemsPrice += Profit + (BoughtPrice / ItemQuantity);

            return (BoughtPrice - soldItemsPrice.Value) * -1;
        }

        #endregion
    }
}
