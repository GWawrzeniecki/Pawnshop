﻿using PawnShop.Business.Models;
using PawnShop.Core;
using PawnShop.Core.Dialogs;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.Enums;
using PawnShop.Modules.Contract.Extensions;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Contract.Models.DropDownButtonModels;
using PawnShop.Modules.Contract.Services;
using PawnShop.Modules.Contract.Validators;
using PawnShop.Modules.Contract.Views;
using PawnShop.Modules.Contract.Windows.Views;
using PawnShop.Services.DataService.QueryDataModels;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
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
        private DelegateCommand _refreshCommand;
        private DelegateCommand _createContractCommand;

        #endregion private members

        #region constructor

        public ContractViewModel(IContractService contractService, IDialogService dialogService, IShellService shellService, IContainerProvider containerProvider, ContractValidator contractValidator) : base(contractValidator)
        {
            Contracts = new List<Business.Models.Contract>();
            this._contractService = contractService;
            this._dialogService = dialogService;
            _shellService = shellService;
            _containerProvider = containerProvider;
            LoadStartupData();
        }

        #endregion constructor

        #region properties

        public IList<Business.Models.Contract> Contracts
        {
            get => _contracts;
            set => SetProperty(ref _contracts, value);
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

        public DelegateCommand<object> DateSearchOptionCommand => _dateSearchOptionCommand ??= new DelegateCommand<object>(SetSearchOption);
        public DelegateCommand<object> RefreshButtonOptionCommand => _refreshButtonCommand ??= new DelegateCommand<object>(SetRefreshButtonOption);
        public DelegateCommand RefreshCommand => _refreshCommand ??= new DelegateCommand(RefreshDataGrid);
        public DelegateCommand CreateContractCommand => _createContractCommand ??= new DelegateCommand(CreateContract);

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
                _dialogService.ShowNotificationDialog("Błąd", $"{loadingContractStateException.Message}{Environment.NewLine}Błąd: {loadingContractStateException.InnerException?.Message}", null);
            }
            catch (LoadingLendingRatesException laodingLendingRateException)
            {
                _dialogService.ShowNotificationDialog("Błąd", $"{laodingLendingRateException.Message}{Environment.NewLine}Błąd: {laodingLendingRateException.InnerException?.Message}", null);
            }
            catch (LoadingContractsException loadingContractsException)
            {
                _dialogService.ShowNotificationDialog("Błąd", $"{loadingContractsException.Message}{Environment.NewLine}Błąd: {loadingContractsException.InnerException?.Message}", null);
            }
            catch (Exception e)
            {
                _dialogService.ShowNotificationDialog("Błąd", $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}", null);
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
                new DateSearchOption {Name = "Wyczyść", SearchOption = SearchOptions.Clean},
                new DateSearchOption {Name = "Dzisiaj", SearchOption = SearchOptions.Today},
                new DateSearchOption {Name = "Wczoraj", SearchOption = SearchOptions.Yesterday},
                new DateSearchOption {Name = "Bieżący tydzien", SearchOption = SearchOptions.CurrentWeek},
                new DateSearchOption {Name = "Poprzedni tydzien", SearchOption = SearchOptions.PastWeek},
                new DateSearchOption {Name = "Bieżący miesiąc", SearchOption = SearchOptions.CurrentMonth},
                new DateSearchOption {Name = "Poprzedni miesiąc", SearchOption = SearchOptions.PastMonth},
                new DateSearchOption {Name = "Bieżący kwartał", SearchOption = SearchOptions.CurrentQuarter},
                new DateSearchOption {Name = "Poprzedni kwartał", SearchOption = SearchOptions.PastQuarter},
                new DateSearchOption {Name = "Bieżący rok", SearchOption = SearchOptions.CurrentYear},
                new DateSearchOption {Name = "Poprzedni rok", SearchOption = SearchOptions.PastYear},
            };
        }

        private void LoadRefreshButtonOptions()
        {
            RefreshButtonOptions = new List<RefreshButtonOption>
            {
                new RefreshButtonOption {Name = "Wyczyść filtr", RefreshOption = RefreshOptions.Clean},
                new RefreshButtonOption {Name = "Wyczyść filtr i odśwież", RefreshOption = RefreshOptions.CleanAndRefresh}
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
                var queryData = new ContractQueryData
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
                _dialogService.ShowNotificationDialog("Błąd", $"{loadingContractsException.Message}{Environment.NewLine}Błąd: {loadingContractsException.InnerException?.Message}", null);
            }
            catch (Exception e)
            {
                _dialogService.ShowNotificationDialog("Błąd", $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}", null);
            }
        }

        private async Task TryToRefreshDataGrid(ContractQueryData queryData)
        {
            Contracts = await _contractService.GetContracts(queryData, 100);
        }

        private void CreateContract()
        {
            var scopedRegion = _shellService.ShowShell<CreateContractWindow>(nameof(ClientData));
            var clientDataHamburgerMenuItem = _containerProvider.Resolve<ClientDataHamburgerMenuItem>();
            RegionManagerAware.SetRegionManagerAware(clientDataHamburgerMenuItem, scopedRegion);
            scopedRegion.Regions[RegionNames.MenuRegion].Add(clientDataHamburgerMenuItem);
        }

        #endregion private methods

        #region viewModelBase

        protected override ContractViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase
    }
}