using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using PawnShop.Core.Models.DropDownButtonModels;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Commodity.Events;
using PawnShop.Modules.Commodity.RegionContext;
using PawnShop.Modules.Commodity.Validators;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Commodity.ViewModels
{
    public class CommodityViewModel : ViewModelBase<CommodityViewModel>
    {
        #region PrivateMembers

        private string _itemName;
        private string _client;
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private IList<ContractItemCategory> _contractItemCategories;
        private ContractItemCategory _selectedContractItemCategory;
        private string _price;
        private IList<RefreshButtonOption> _refreshButtonOptions;
        private IList<DateSearchOption> _dateSearchOptions;
        private DelegateCommand<object> _refreshButtonCommand;
        private DelegateCommand _refreshCommand;
        private string _contractNumber;
        private IList<SearchPriceOption> _priceOptions;
        private readonly IMapper _mapper;
        private readonly IContainerProvider _containerProvider;
        private readonly IMessageBoxService _messageBoxService;
        private readonly RefreshDataGridEvent _refreshDataGridEvent;
        private readonly TaskBarButtonClickEvent _taskBarButtonClickEvent;
        private DelegateCommand _previewCommand;
        private DelegateCommand _putOnSaleCommand;
        private DelegateCommand<DateSearchOption> _dateSearchOptionCommand;
        private CommodityTabRegionContext _commodityTabRegionContext;

        #endregion

        #region Constructor
        public CommodityViewModel(IMapper mapper, IEventAggregator eventAggregator, IContainerProvider containerProvider, CommodityValidator commodityValidator, IMessageBoxService messageBoxService) : base(commodityValidator)
        {
            _mapper = mapper;
            _containerProvider = containerProvider;
            _messageBoxService = messageBoxService;
            _refreshDataGridEvent = eventAggregator.GetEvent<RefreshDataGridEvent>();
            _taskBarButtonClickEvent = eventAggregator.GetEvent<TaskBarButtonClickEvent>();
            CommodityTabRegionContext = new CommodityTabRegionContext();
            LoadStartupData();
        }

        #endregion

        #region PublicProperties

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

        public CommodityTabRegionContext CommodityTabRegionContext
        {
            get => _commodityTabRegionContext;
            set => SetProperty(ref _commodityTabRegionContext, value);
        }

        public SearchPriceOption SelectedPriceOption { get; set; }

        #endregion

        #region ViewModelBase

        protected override CommodityViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase

        #region Commands
        public DelegateCommand<DateSearchOption> DateSearchOptionCommand =>
            _dateSearchOptionCommand ??= new DelegateCommand<DateSearchOption>(ModelsLoader.SetSearchOption);

        public DelegateCommand<object> RefreshButtonOptionCommand =>
            _refreshButtonCommand ??= new DelegateCommand<object>(SetRefreshButtonOption);

        public DelegateCommand RefreshCommand => _refreshCommand ??= new DelegateCommand(RefreshDataGrid, CanExecuteRefresh)
                .ObservesProperty(() => HasErrors);

        public DelegateCommand PreviewCommand => _previewCommand ??= new DelegateCommand(ShowPreview, CanExecutePreviewPutOnSale)
                .ObservesProperty(() => CommodityTabRegionContext.CanExecute);

        public DelegateCommand PutOnSaleCommand => _putOnSaleCommand ??= new DelegateCommand(PutOnSale, CanExecutePreviewPutOnSale)
            .ObservesProperty(() => CommodityTabRegionContext.CanExecute);

        #endregion

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

        private void RefreshDataGrid()
        {
            var goodsQueryData = _mapper.Map<ContractItemQueryData>(this);
            _refreshDataGridEvent.Publish(goodsQueryData);
        }

        private void ShowPreview()
        {
            _taskBarButtonClickEvent.Publish(DialogMode.ReadOnly);
        }

        private void PutOnSale()
        {
            _taskBarButtonClickEvent.Publish(DialogMode.Editable);
        }

        private bool CanExecuteRefresh()
        {
            return !HasErrors;
        }

        #endregion

        #region PrivateMethods

        private async void LoadStartupData()
        {
            try
            {
                await TryToLoadContractItemCategories();
                LoadDateSearchOptions();
                LoadRefreshButtonOptions();
                LoadPriceOptions();
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

        private bool CanExecutePreviewPutOnSale()
        {
            return CommodityTabRegionContext.CanExecute;
        }

        #endregion
    }
}
