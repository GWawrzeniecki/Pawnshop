using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Core.Dialogs;
using PawnShop.Core.Enums;
using PawnShop.Core.Models.DropDownButtonModels;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Sale.Validators;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PawnShop.Modules.Sale.ViewModels
{
    public class SaleViewModel : ViewModelBase<SaleViewModel>
    {
        #region PrivateMembers

        private bool _isBusy;
        private string _itemName;
        private string _client;
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private IList<ContractItemCategory> _contractItemCategories;
        private ContractItemCategory _selectedContractItemCategory;
        private string _price;
        private IList<RefreshButtonOption> _refreshButtonOptions;
        private IList<DateSearchOption> _dateSearchOptions;
        private IList<Business.Models.Sale> _sales;
        private Business.Models.Sale _selectedSale;
        private DelegateCommand<object> _refreshButtonCommand;
        private DelegateCommand _refreshCommand;
        private string _contractNumber;
        private IList<SearchPriceOption> _priceOptions;
        private DelegateCommand<DateSearchOption> _dateSearchOptionCommand;
        private readonly IContainerProvider _containerProvider;
        private readonly IMapper _mapper;
        private readonly IDialogService _dialogService;
        private readonly IMessageBoxService _messageBoxService;
        private DelegateCommand _showPreviewCommand;
        private DelegateCommand _sellCommand;

        #endregion

        #region Constructor

        public SaleViewModel(IContainerProvider containerProvider, IMapper mapper, IDialogService dialogService, SaleValidator saleValidator, IMessageBoxService messageBoxService) : base(saleValidator)
        {
            _containerProvider = containerProvider;
            _mapper = mapper;
            _dialogService = dialogService;
            _messageBoxService = messageBoxService;
            Sales = new List<Business.Models.Sale>();
            LoadStartupData();
        }

        #endregion

        #region PublicProperties

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string ItemName
        {
            get => _itemName;
            set => SetProperty(ref _itemName, value);
        }

        public string Client
        {
            get => _client;
            set => SetProperty(ref _client, value);
        }

        public DateTime? FromDate
        {
            get => _fromDate;
            set => SetProperty(ref _fromDate, value);
        }

        public DateTime? ToDate
        {
            get => _toDate;
            set => SetProperty(ref _toDate, value);
        }

        public IList<ContractItemCategory> ContractItemCategories
        {
            get => _contractItemCategories;
            set => SetProperty(ref _contractItemCategories, value);
        }

        public ContractItemCategory SelectedContractItemCategory
        {
            get => _selectedContractItemCategory;
            set => SetProperty(ref _selectedContractItemCategory, value);
        }

        public string Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public IList<RefreshButtonOption> RefreshButtonOptions
        {
            get => _refreshButtonOptions;
            set => SetProperty(ref _refreshButtonOptions, value);
        }

        public IList<DateSearchOption> DateSearchOptions
        {
            get => _dateSearchOptions;
            set => SetProperty(ref _dateSearchOptions, value);
        }

        public IList<Business.Models.Sale> Sales
        {
            get => _sales;
            set => SetProperty(ref _sales, value);
        }

        public Business.Models.Sale SelectedSale
        {
            get => _selectedSale;
            set => SetProperty(ref _selectedSale, value);
        }

        public string ContractNumber
        {
            get => _contractNumber;
            set => SetProperty(ref _contractNumber, value);
        }

        public IList<SearchPriceOption> PriceOptions
        {
            get => _priceOptions;
            set => SetProperty(ref _priceOptions, value);
        }

        public SearchPriceOption SelectedPriceOption { get; set; }

        #endregion

        #region Commands

        public DelegateCommand<DateSearchOption> DateSearchOptionCommand => _dateSearchOptionCommand ??= new DelegateCommand<DateSearchOption>(ModelsLoader.SetSearchOption);

        public DelegateCommand<object> RefreshButtonOptionCommand => _refreshButtonCommand ??= new DelegateCommand<object>(SetRefreshButtonOption);


        public DelegateCommand RefreshCommand => _refreshCommand ??= new DelegateCommand(RefreshDataGridAsync, CanExecuteRefresh)
                .ObservesProperty(() => HasErrors);

        public DelegateCommand ShowPreviewCommand => _showPreviewCommand ??= new DelegateCommand(ShowPreview, () => SelectedSale is not null)
            .ObservesProperty(() => SelectedSale);

        public DelegateCommand SellCommand => _sellCommand ??= new DelegateCommand(Sell, () => SelectedSale is not null && !SelectedSale.IsSold)
            .ObservesProperty(() => SelectedSale);


        #endregion

        #region viewModelBase

        protected override SaleViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase

        #region CommandsMethods

        private void SetRefreshButtonOption(object refreshOption)
        {
            switch (refreshOption)
            {
                case RefreshOptions.Clean:
                    CleanSearchProperties();
                    break;

                case RefreshOptions.CleanAndRefresh:
                    CleanSearchProperties();
                    RefreshCommand.Execute();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(refreshOption), refreshOption, null);
            }
        }

        private async void RefreshDataGridAsync()
        {
            try
            {
                IsBusy = true;
                var salesQueryData = _mapper.Map<ContractItemQueryData>(this);
                await TryToLoadSales(salesQueryData);
            }
            catch (LoadingSalesException loadingSalesException)
            {
                _messageBoxService.ShowError(
                    $"{loadingSalesException.Message}{Environment.NewLine}Błąd: {loadingSalesException.InnerException?.Message}",
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
                IsBusy = false;
            }
        }

        private async Task RefreshDataGrid()
        {
            try
            {
                IsBusy = true;
                var salesQueryData = _mapper.Map<ContractItemQueryData>(this);
                await TryToLoadSales(salesQueryData);
            }
            catch (LoadingSalesException loadingSalesException)
            {
                _messageBoxService.ShowError(
                    $"{loadingSalesException.Message}{Environment.NewLine}Błąd: {loadingSalesException.InnerException?.Message}",
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
                IsBusy = false;
            }
        }

        private bool CanExecuteRefresh()
        {
            return !HasErrors;
        }

        private void ShowPreview()
        {
            _dialogService.ShowPreviewSaleDialog(null, "Podgląd sprzedawanego towaru", SelectedSale);
        }

        private async void Sell()
        {
            var dialogResult = ButtonResult.Cancel;
            _dialogService.ShowSaleDialog((result =>
            {
                dialogResult = result.Result;

            }), "Sprzedaż towaru", SelectedSale);

            if (dialogResult != ButtonResult.OK) return;
            await RefreshDataGrid();

        }

        #endregion

        #region PrivateMethods

        private async void LoadStartupData()
        {
            try
            {
                IsBusy = true;
                await TryToLoadSales();
                await TryToLoadContractItemCategories();
                LoadDateSearchOptions();
                LoadRefreshButtonOptions();
                LoadPriceOptions();
            }
            catch (LoadingSalesException loadingSalesException)
            {
                _messageBoxService.ShowError(
                    $"{loadingSalesException.Message}{Environment.NewLine}Błąd: {loadingSalesException.InnerException?.Message}",
                    "Błąd");
            }
            catch (LoadingContractItemCategoriesException loadingContractItemCategoriesException)
            {
                _messageBoxService.ShowError(
                    $"{loadingContractItemCategoriesException.Message}{Environment.NewLine}Błąd: {loadingContractItemCategoriesException.InnerException?.Message}",
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
                IsBusy = false;
            }
        }

        private void LoadRefreshButtonOptions()
        {
            RefreshButtonOptions = ModelsLoader.LoadRefreshButtonOptions();
        }

        private void LoadDateSearchOptions()
        {
            DateSearchOptions = ModelsLoader.LoadDateSearchOptions((fromDate, toDate) =>
            {
                FromDate = fromDate;
                ToDate = toDate;
            });
        }

        private void LoadPriceOptions()
        {
            PriceOptions = ModelsLoader.LoadPriceOptions();
        }

        private void CleanSearchProperties()
        {
            FromDate = null;
            ToDate = null;
            ContractNumber = string.Empty;
            Client = string.Empty;
            Price = string.Empty;
            ItemName = string.Empty;
            SelectedContractItemCategory = null;
            SelectedPriceOption = null;
        }

        private async Task TryToLoadSales()
        {
            try
            {
                using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
                Sales = await unitOfWork.SaleRepository.GetTopSales(100);
            }
            catch (Exception e)
            {
                throw new LoadingSalesException("Wystapil blad podczas ladowania obecnie sprzedawanych towarow.", e);
            }
        }

        private async Task TryToLoadSales(ContractItemQueryData contractItemQueryData)
        {
            try
            {
                using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
                Sales = await unitOfWork.SaleRepository.GetTopSales(contractItemQueryData, 100);
            }
            catch (Exception e)
            {
                throw new LoadingSalesException("Wystapil blad podczas wyszukiwania obecnie sprzedawanych towarow", e);
            }
        }

        private async Task TryToLoadContractItemCategories()
        {
            try
            {
                using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
                ContractItemCategories = (await unitOfWork.ContractItemCategoryRepository.GetAsync()).ToList();
            }
            catch (Exception e)
            {
                throw new LoadingContractItemCategoriesException("Wystapil blad podczas ladowania kategorii towaru.", e);
            }
        }

        #endregion

    }
}
