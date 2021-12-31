using PawnShop.Business.Models;
using PawnShop.Core.ScopedRegion;
using PawnShop.Core.Tasks;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.Interfaces;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class BuyBackContractDataViewModel : BindableBase, IRegionManagerAware, INavigationAware
    {
        #region PrivateMembers

        private Business.Models.Contract _contract;
        private NotifyTask<IList<LendingRate>> _lendingRates;
        private bool _isDelayed;
        private int _howManyDaysLate;
        private LendingRate _selectedDelayLendingRate;
        private LendingRate _actualLendingRate;
        private readonly ICalculateService _calculateService;
        private readonly IContractService _contractService;
        private readonly IMessageBoxService _messageBoxService;
        private DateTime _contractStartDate;
        private Func<Task> _callBack;

        #endregion

        #region Constructor
        public BuyBackContractDataViewModel(ICalculateService calculateService, IContractService contractService, IMessageBoxService messageBoxService)
        {
            _calculateService = calculateService;
            _contractService = contractService;
            _messageBoxService = messageBoxService;
            Contract = new Business.Models.Contract { ContractItems = new List<ContractItem>(), LendingRate = new LendingRate() };
            LendingRates = NotifyTask.Create(LoadLendingRate);
        }

        #endregion

        #region PublicProperties

        public Business.Models.Contract Contract
        {
            get => _contract;
            set
            {
                SetProperty(ref _contract, value);
                RaisePropertyChanged(nameof(ContractDate));
                RaisePropertyChanged(nameof(HowManyDaysLateCalculated));
                RaisePropertyChanged(nameof(SumOfEstimatedValues));
                RaisePropertyChanged(nameof(BuyBackPrice));
            }
        }

        public DateTime ContractDate
        {
            get
            {
                if (Contract.ContractRenews.Count == 0)
                {
                    ActualLendingRate = Contract.LendingRate;
                    RaisePropertyChanged(nameof(BuyBackPrice));
                    ContractStartDate = Contract.StartDate;
                    return Contract.StartDate.AddDays(Contract.LendingRate.Days);
                }

                var lastRenew = Contract.ContractRenews
                    .OrderByDescending(c => c.RenewContractId)
                    .First();
                ActualLendingRate = lastRenew.LendingRate;
                RaisePropertyChanged(nameof(BuyBackPrice));
                ContractStartDate = lastRenew.StartDate;
                return lastRenew.StartDate.AddDays(lastRenew.LendingRate.Days);
            }
        }

        private LendingRate ActualLendingRate
        {
            get => _actualLendingRate;
            set => SetProperty(ref _actualLendingRate, value);
        }

        public DateTime ContractStartDate
        {
            get => _contractStartDate;
            set => SetProperty(ref _contractStartDate, value);
        }

        public NotifyTask<int> HowManyDaysLateCalculated => NotifyTask.Create(GetHowManyDaysCalculated);

        public bool IsDelayed
        {
            get => _isDelayed;
            set => SetProperty(ref _isDelayed, value);
        }

        public int HowManyDaysLate
        {
            get => _howManyDaysLate;
            set
            {
                SetProperty(ref _howManyDaysLate, value);
                RaisePropertyChanged(nameof(BuyBackPrice));
            }
        }

        public decimal SumOfEstimatedValues => Contract.ContractItems.Sum(c => c.EstimatedValue);

        public NotifyTask<IList<LendingRate>> LendingRates
        {
            get => _lendingRates;
            set
            {
                SetProperty(ref _lendingRates, value);
                RaisePropertyChanged(nameof(BuyBackPrice));
            }
        }

        public LendingRate SelectedDelayLendingRate
        {
            get => _selectedDelayLendingRate;
            set
            {
                SetProperty(ref _selectedDelayLendingRate, value);
                HowManyDaysLate = value?.Days ?? 0;
            }
        }

        public NotifyTask<decimal> BuyBackPrice => NotifyTask.Create(GetBuyBackPrice);

        #endregion

        #region PrivateMethods

        private async Task<IList<LendingRate>> LoadLendingRate()
        {
            try
            {
                return await _contractService.LoadLendingRates();
            }
            catch (LoadingLendingRatesException loadingLendingRatesException)
            {
                _messageBoxService.ShowError(
                    $"{loadingLendingRatesException.Message}{Environment.NewLine}Błąd: {loadingLendingRatesException.InnerException?.Message}",
                    "Błąd");
            }

            return Enumerable.Empty<LendingRate>().ToList();
        }

        private async Task<int> GetHowManyDaysCalculated()
        {
            var days = DateTime.Compare(ContractDate, DateTime.Now) < 0 ? DateTime.Today.Subtract(ContractDate).Days : 0;
            IsDelayed = days > 0;
            HowManyDaysLate = days;
            SelectedDelayLendingRate = (await LendingRates.Task)?.FirstOrDefault(lr => lr.Days == days) ?? new LendingRate { Days = days };
            return days;
        }

        private async Task<decimal> GetBuyBackPrice()
        {
            return ActualLendingRate is not null && LendingRates is not null
                 ? _calculateService.CalculateBuyBackCost(SumOfEstimatedValues, ActualLendingRate, HowManyDaysLate, await LendingRates.Task)
                 : 0;
        }

        #endregion

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var contract = navigationContext.Parameters.GetValue<Business.Models.Contract>("contract");
            if (contract is not null)
                Contract = contract;
            _callBack = navigationContext.Parameters.GetValue<Func<Task>>("CallBack");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add("contract", Contract);
            navigationContext.Parameters.Add("buyBackPrice", BuyBackPrice.Result);
            navigationContext.Parameters.Add("CallBack", _callBack);
        }

        #endregion
    }
}
