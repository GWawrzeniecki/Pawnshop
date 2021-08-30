using BespokeFusion;
using PawnShop.Business.Models;
using PawnShop.Core.SharedVariables;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.Enums;
using PawnShop.Modules.Contract.Extensions;
using PawnShop.Modules.Contract.Models.DropDownButtonModels;
using PawnShop.Modules.Contract.Validators;
using PawnShop.Modules.Contract.Views;
using PawnShop.Modules.Contract.Windows.Views;
using PawnShop.Services.DataService.QueryDataModels;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class ContractViewModel : ViewModelBase<ContractViewModel>
    {
        #region private members

        private IList<Business.Models.Contract> _contracts;
        private readonly IContractService _contractService;
        private readonly IDialogService _dialogService;
        private readonly IShellService _shellService;
        private readonly IContainerProvider _containerProvider;
        private readonly ISessionContext _sessionContext;
        private IList<LendingRate> _lendingRates;
        private IList<ContractState> _contractStates;
        private IList<DateSearchOption> _dateSearchOptions;
        private DelegateCommand<object> _dateSearchOptionCommand;
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

        #endregion private members

        #region constructor

        public ContractViewModel(IContractService contractService, IDialogService dialogService,
            IShellService shellService, IContainerProvider containerProvider, ContractValidator contractValidator, ISessionContext sessionContext) :
            base(contractValidator)
        {
            Contracts = new List<Business.Models.Contract>();
            _contractService = contractService;
            _dialogService = dialogService;
            _shellService = shellService;
            _containerProvider = containerProvider;
            _sessionContext = sessionContext;
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



        #endregion properties

        #region commands

        public DelegateCommand<object> DateSearchOptionCommand =>
            _dateSearchOptionCommand ??= new DelegateCommand<object>(SetSearchOption);

        public DelegateCommand<object> RefreshButtonOptionCommand =>
            _refreshButtonCommand ??= new DelegateCommand<object>(SetRefreshButtonOption);

        public DelegateCommand RefreshCommand => _refreshCommand ??= new DelegateCommand(RefreshDataGrid);
        public DelegateCommand CreateContractCommand => _createContractCommand ??= new DelegateCommand(CreateContract);
        public DelegateCommand RenewContractCommand => _renewContractCommand ??= new DelegateCommand(RenewContract);
        public DelegateCommand BuyBackContractCommand => _buyBackContractCommand ??= new DelegateCommand(BuyBackContract);

        #endregion commands

        #region private methods

        private async void LoadStartupData()
        {
            try
            {
                await TryToLoadContractStates();
                await TryToLoadLendingRate();
                await TryToLoadContracts();
                LoadDateSearchOptions();
                LoadRefreshButtonOptions();
            }
            catch (LoadingContractStatesException loadingContractStateException)
            {
                MaterialMessageBox.ShowError(
                    $"{loadingContractStateException.Message}{Environment.NewLine}Błąd: {loadingContractStateException.InnerException?.Message}",
                    "Błąd");
            }
            catch (LoadingLendingRatesException laodingLendingRateException)
            {
                MaterialMessageBox.ShowError(
                    $"{laodingLendingRateException.Message}{Environment.NewLine}Błąd: {laodingLendingRateException.InnerException?.Message}",
                    "Błąd");
            }
            catch (LoadingContractsException loadingContractsException)
            {
                MaterialMessageBox.ShowError(
                    $"{loadingContractsException.Message}{Environment.NewLine}Błąd: {loadingContractsException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
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
            Contracts = (await _contractService.LoadContracts())
                .ToList();
        }

        private void LoadDateSearchOptions()
        {
            DateSearchOptions = new List<DateSearchOption>
            {
                new() {Name = "Wyczyść", SearchOption = SearchOptions.Clean},
                new() {Name = "Dzisiaj", SearchOption = SearchOptions.Today},
                new() {Name = "Wczoraj", SearchOption = SearchOptions.Yesterday},
                new() {Name = "Bieżący tydzien", SearchOption = SearchOptions.CurrentWeek},
                new() {Name = "Poprzedni tydzien", SearchOption = SearchOptions.PastWeek},
                new() {Name = "Bieżący miesiąc", SearchOption = SearchOptions.CurrentMonth},
                new() {Name = "Poprzedni miesiąc", SearchOption = SearchOptions.PastMonth},
                new() {Name = "Bieżący kwartał", SearchOption = SearchOptions.CurrentQuarter},
                new() {Name = "Poprzedni kwartał", SearchOption = SearchOptions.PastQuarter},
                new() {Name = "Bieżący rok", SearchOption = SearchOptions.CurrentYear},
                new() {Name = "Poprzedni rok", SearchOption = SearchOptions.PastYear}
            };
        }

        private void LoadRefreshButtonOptions()
        {
            RefreshButtonOptions = new List<RefreshButtonOption>
            {
                new() {Name = "Wyczyść filtr", RefreshOption = RefreshOptions.Clean},
                new() {Name = "Wyczyść filtr i odśwież", RefreshOption = RefreshOptions.CleanAndRefresh}
            };
        }

        private void SetSearchOption(object searchOption)
        {
            switch (searchOption)
            {
                case SearchOptions.Clean:
                    FromDate = null;
                    ToDate = null;
                    break;

                case SearchOptions.Today:
                    FromDate = DateTime.Today;
                    ToDate = DateTime.Today;
                    break;

                case SearchOptions.Yesterday:
                    FromDate = DateTime.Today.Yesterday();
                    ToDate = DateTime.Today.Yesterday();
                    break;

                case SearchOptions.CurrentWeek:
                    FromDate = DateTime.Today.Monday();
                    ToDate = DateTime.Today.Sunday();
                    break;

                case SearchOptions.PastWeek:
                    FromDate = DateTime.Today.PastMonday();
                    ToDate = DateTime.Today.PastSunday();
                    break;

                case SearchOptions.CurrentMonth:
                    FromDate = DateTime.Today.BeginningOfCurrentMonth();
                    ToDate = DateTime.Today.EndOfCurrentMonth();
                    break;

                case SearchOptions.PastMonth:
                    FromDate = DateTime.Today.BeginningOfPastMonth();
                    ToDate = DateTime.Today.EndOfPastMonth();
                    break;

                case SearchOptions.CurrentQuarter:
                    FromDate = DateTime.Today.BeginningOfCurrentQuarter();
                    ToDate = DateTime.Today.EndOfCurrentQuarter();
                    break;

                case SearchOptions.PastQuarter:
                    FromDate = DateTime.Today.BeginningOfPastQuarter();
                    ToDate = DateTime.Today.EndOfPastQuarter();
                    break;

                case SearchOptions.CurrentYear:
                    FromDate = DateTime.Today.BeginningOfCurrentYear();
                    ToDate = DateTime.Today.EndOfCurrentYear();
                    break;

                case SearchOptions.PastYear:
                    FromDate = DateTime.Today.BeginningOfPastYear();
                    ToDate = DateTime.Today.EndOfPastYear();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(searchOption), searchOption, null);
            }
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

        private async void RefreshDataGrid()
        {
            try
            {
                var queryData = new ContractQueryData // to do mapping
                {
                    FromDate = FromDate,
                    ToDate = ToDate,
                    Client = Client,
                    ContractAmount = ContractAmount,
                    ContractNumber = ContractNumber,
                    ContractState = ContractState,
                    LendingRate = LendingRate
                };

                await TryToRefreshDataGrid(queryData);
            }
            catch (LoadingContractsException loadingContractsException)
            {
                MaterialMessageBox.ShowError(
                    $"{loadingContractsException.Message}{Environment.NewLine}Błąd: {loadingContractsException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private async Task TryToRefreshDataGrid(ContractQueryData queryData)
        {
            Contracts = await _contractService.GetContracts(queryData, 100); // to do
        }

        private void CreateContract()
        {
            _shellService.ShowShell<CreateContractWindow>(nameof(ClientData));
        }

        private void RenewContract()
        {
            if (SelectedContract is null || SelectedContract.ContractState.State.Equals(Core.Constants.Constants.BuyBackContractState))
                return;
            _shellService.ShowShell<RenewContractWindow>(nameof(RenewContractData), new NavigationParameters() { { "contract", SelectedContract } });

        }

        private void BuyBackContract()
        {
            if (SelectedContract is null || SelectedContract.ContractState.State.Equals(Core.Constants.Constants.BuyBackContractState))
                return;
            _shellService.ShowShell<BuyBackContractWindow>(nameof(BuyBackContractData), new NavigationParameters() { { "contract", SelectedContract } });
        }

        #endregion private methods
    }
}