using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Core.Constants;
using PawnShop.Core.Enums;
using PawnShop.Core.Models.DropDownButtonModels;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.Core.SharedVariables;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.Validators;
using PawnShop.Modules.Contract.Views;
using PawnShop.Modules.Contract.Windows.Views;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class ContractViewModel : ViewModelBase<ContractViewModel>
    {
        #region private members

        private IList<Business.Models.Contract> _contracts;
        private readonly IContractService _contractService;
        private readonly IDialogService _dialogService;
        private readonly IUserSettings _userSettings;
        private readonly IShellService _shellService;
        private readonly IContainerProvider _containerProvider;
        private readonly ISessionContext _sessionContext;
        private readonly IMessageBoxService _messageBoxService;
        private readonly IMapper _mapper;
        private IList<LendingRate> _lendingRates;
        private IList<ContractState> _contractStates;
        private IList<DateSearchOption> _dateSearchOptions;
        private DelegateCommand<DateSearchOption> _dateSearchOptionCommand;
        private DelegateCommand<object> _refreshButtonCommand;
        private IList<RefreshButtonOption> _refreshButtonOptions;
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private string _contractNumber;
        private ContractState _contractState;
        private string _client;
        private string _contractAmount;
        private LendingRate _lendingRate;
        private Business.Models.Contract _selectedContract;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _createContractCommand;
        private DelegateCommand _renewContractCommand;
        private DelegateCommand _buyBackContractCommand;
        private bool _isBusy;

        #endregion private members

        #region constructor

        public ContractViewModel(IContractService contractService, IDialogService dialogService, IUserSettings userSettings,
            IShellService shellService, IContainerProvider containerProvider, ContractValidator contractValidator, ISessionContext sessionContext, IMessageBoxService messageBoxService, IMapper mapper) :
            base(contractValidator)
        {
            Contracts = new List<Business.Models.Contract>();
            _contractService = contractService;
            _dialogService = dialogService;
            _userSettings = userSettings;
            _shellService = shellService;
            _containerProvider = containerProvider;
            _sessionContext = sessionContext;
            _messageBoxService = messageBoxService;
            _mapper = mapper;
            LoadStartupData();

        }

        #endregion constructor

        #region viewModelBase

        protected override ContractViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase

        #region properties

        public IList<Business.Models.Contract> Contracts
        {
            get => _contracts;
            set => SetProperty(ref _contracts, value);
        }

        public Business.Models.Contract SelectedContract
        {
            get => _selectedContract;
            set => SetProperty(ref _selectedContract, value);
        }

        public IList<LendingRate> LendingRates
        {
            get => _lendingRates;
            set => SetProperty(ref _lendingRates, value);
        }

        public IList<ContractState> ContractStates
        {
            get => _contractStates;
            set => SetProperty(ref _contractStates, value);
        }

        public IList<DateSearchOption> DateSearchOptions
        {
            get => _dateSearchOptions;
            set => SetProperty(ref _dateSearchOptions, value);
        }

        public IList<RefreshButtonOption> RefreshButtonOptions
        {
            get => _refreshButtonOptions;
            set => SetProperty(ref _refreshButtonOptions, value);
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

        public string ContractNumber
        {
            get => _contractNumber;
            set => SetProperty(ref _contractNumber, value);
        }

        public ContractState ContractState
        {
            get => _contractState;
            set => SetProperty(ref _contractState, value);
        }

        public string Client
        {
            get => _client;
            set => SetProperty(ref _client, value);
        }

        public string ContractAmount
        {
            get => _contractAmount;
            set => SetProperty(ref _contractAmount, value);
        }

        public LendingRate LendingRate
        {
            get => _lendingRate;
            set => SetProperty(ref _lendingRate, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion properties

        #region commands

        public DelegateCommand<DateSearchOption> DateSearchOptionCommand => _dateSearchOptionCommand ??= new DelegateCommand<DateSearchOption>(ModelsLoader.SetSearchOption);
        public DelegateCommand<object> RefreshButtonOptionCommand => _refreshButtonCommand ??= new DelegateCommand<object>(SetRefreshButtonOption);
        public DelegateCommand RefreshCommand => _refreshCommand ??= new DelegateCommand(RefreshDataGridAsync);
        public DelegateCommand CreateContractCommand => _createContractCommand ??= new DelegateCommand(CreateContract);

        public DelegateCommand RenewContractCommand => _renewContractCommand ??=
            new DelegateCommand(RenewContract, CanExecuteRenewBuyBackContract)
                .ObservesProperty(() => SelectedContract);

        public DelegateCommand BuyBackContractCommand => _buyBackContractCommand ??=
            new DelegateCommand(BuyBackContract, CanExecuteRenewBuyBackContract)
                .ObservesProperty(() => SelectedContract);

        #endregion commands

        #region private methods

        private async void LoadStartupData()
        {
            try
            {
                IsBusy = true;
                await TryToLoadContractStates();
                await TryToLoadLendingRate();
                await TryToLoadContracts();
                LoadDateSearchOptions();
                LoadRefreshButtonOptions();
            }
            catch (LoadingContractStatesException loadingContractStateException)
            {
                _messageBoxService.ShowError(
                    $"{loadingContractStateException.Message}{Environment.NewLine}Błąd: {loadingContractStateException.InnerException?.Message}",
                    "Błąd");
            }
            catch (LoadingLendingRatesException laodingLendingRateException)
            {
                _messageBoxService.ShowError(
                    $"{laodingLendingRateException.Message}{Environment.NewLine}Błąd: {laodingLendingRateException.InnerException?.Message}",
                    "Błąd");
            }
            catch (LoadingContractsException loadingContractsException)
            {
                _messageBoxService.ShowError(
                    $"{loadingContractsException.Message}{Environment.NewLine}Błąd: {loadingContractsException.InnerException?.Message}",
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

        private async Task TryToLoadContractStates()
        {
            ContractStates = await _contractService.LoadContractStates();
        }

        private async Task TryToLoadLendingRate()
        {
            LendingRates = await _contractService.LoadLendingRates();
        }

        private async Task TryToLoadContracts()
        {
            Contracts = (await _contractService.LoadContracts(100));
        }

        private void LoadDateSearchOptions()
        {
            DateSearchOptions = ModelsLoader.LoadDateSearchOptions((fromDate, toDate) =>
            {
                FromDate = fromDate;
                ToDate = toDate;
            });
        }

        private void LoadRefreshButtonOptions()
        {
            RefreshButtonOptions = ModelsLoader.LoadRefreshButtonOptions();
        }

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

        private void CleanSearchProperties()
        {
            FromDate = null;
            ToDate = null;
            ContractNumber = string.Empty;
            ContractState = null;
            Client = string.Empty;
            ContractAmount = string.Empty;
            LendingRate = null;
        }

        private async void RefreshDataGridAsync()
        {
            try
            {
                IsBusy = true;
                var queryData = _mapper.Map<ContractQueryData>(this);
                await TryToRefreshDataGrid(queryData);
            }
            catch (LoadingContractsException loadingContractsException)
            {
                _messageBoxService.ShowError(
                    $"{loadingContractsException.Message}{Environment.NewLine}Błąd: {loadingContractsException.InnerException?.Message}",
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
                var queryData = _mapper.Map<ContractQueryData>(this);

                await TryToRefreshDataGrid(queryData);
            }
            catch (LoadingContractsException loadingContractsException)
            {
                _messageBoxService.ShowError(
                    $"{loadingContractsException.Message}{Environment.NewLine}Błąd: {loadingContractsException.InnerException?.Message}",
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

        private async Task TryToRefreshDataGrid(ContractQueryData queryData)
        {
            Contracts = await _contractService.GetContracts(queryData, 100);
        }

        private void CreateContract()
        {
            if (CheckIfApplicationIsConfigured())
                _shellService.ShowShell<CreateContractWindow>(nameof(CreateContractClientData), new NavigationParameters { { "CallBack", RefreshDataGridCallBack() } });
            else
                _messageBoxService.ShowError($"Aplikacja nie jest skonfigurowana.Sprawdź następujące ustawienia:{Environment.NewLine}1. Ścieżka do szablonu z umową.{Environment.NewLine}2. Stawka VAT.{Environment.NewLine}3. Folder na umowy.", "Uwaga!");
        }

        private Func<Task> RefreshDataGridCallBack()
        {
            return RefreshDataGrid;
        }

        private void RenewContract()
        {
            _shellService.ShowShell<RenewContractWindow>(nameof(RenewContractData), new NavigationParameters { { "contract", SelectedContract }, { "CallBack", RefreshDataGridCallBack() } });
        }

        private void BuyBackContract()
        {
            _shellService.ShowShell<BuyBackContractWindow>(nameof(BuyBackContractData), new NavigationParameters { { "contract", SelectedContract }, { "CallBack", RefreshDataGridCallBack() } });
        }

        private bool CanExecuteRenewBuyBackContract()
        {
            return SelectedContract is not null &&
                    !SelectedContract.ContractState.State.Equals(Constants.BoughtBackContractState);
        }

        private bool CheckIfApplicationIsConfigured()
        {
            return !string.IsNullOrEmpty(_userSettings.DealDocumentPath) &&
                   !string.IsNullOrEmpty(_userSettings.DealDocumentsFolderPath) &&
                   _userSettings.VatPercent != 0;

        }

        #endregion private methods
    }
}