using Prism.Mvvm;
using System.Collections.Generic;
using PawnShop.Business.Models;
using System;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.Services;
using System.Threading.Tasks;
using Prism.Services.Dialogs;
using PawnShop.Core.Dialogs;
using System.Linq;
using PawnShop.Modules.Contract.Enums;
using PawnShop.Modules.Contract.Extensions;
using PawnShop.Modules.Contract.Models.DropDownButtonModels;
using Prism.Commands;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class ContractViewModel : BindableBase
    {
        #region private members
        private List<Business.Models.Contract> _contracts;
        private readonly IContractService _contractService;
        private readonly IDialogService _dialogService;
        private IList<LendingRate> _lendingRates;
        private IList<ContractState> _contractStates;
        private IList<DateSearchOption> _dateSearchOptions;
        private DelegateCommand<object> _dateSearchOptionCommand;
        private DateTime? _fromDate;
        private DateTime? _toDate;
        #endregion


        #region constructor
        public ContractViewModel(IContractService contractService, IDialogService dialogService)
        {
            Contracts = new List<Business.Models.Contract>();
            this._contractService = contractService;
            this._dialogService = dialogService;
            LoadStartupData();
        }


        #endregion


        #region properties
        public List<Business.Models.Contract> Contracts
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

        #endregion


        #region comands

        public DelegateCommand<object> DateSearchOptionCommand => _dateSearchOptionCommand ??= new DelegateCommand<object>(SetSearchOption);



        #endregion

        #region private methods

        private async void LoadStartupData()
        {
            try
            {
                await TryToLoadContractStates();
                await TryToLoadLendingRate();
                await TryToLoadContracts();
                LoadDateSearchOptions();
            }
            catch (LoadingContractStatesException loadingContractStateException)
            {
                _dialogService.ShowNotificationDialog("Błąd", $"{loadingContractStateException.Message}{Environment.NewLine}Błąd: {loadingContractStateException.InnerException?.Message}", null);
            }
            catch (LoadingLendingRatesException laodingLendingRateException)
            {
                _dialogService.ShowNotificationDialog("Błąd", $"{laodingLendingRateException.Message}{Environment.NewLine}Błąd: {laodingLendingRateException.InnerException?.Message}", null);
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
                    break;
                case SearchOptions.CurrentMonth:
                    break;
                case SearchOptions.PastMonth:
                    break;
                case SearchOptions.CurrentQuarter:
                    break;
                case SearchOptions.PastQuarter:
                    break;
                case SearchOptions.CurrentYear:
                    break;
                case SearchOptions.PastYear:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchOption), searchOption, null);
            }
        }

        #endregion

    }
}