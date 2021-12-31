using PawnShop.Business.Models;
using PawnShop.Core.HamburgerMenu.Implementations;
using PawnShop.Core.HamburgerMenu.Interfaces;
using PawnShop.Core.ScopedRegion;
using PawnShop.Core.Tasks;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class RenewContractDataViewModel : BindableBase, IRegionManagerAware, INavigationAware, IHamburgerMenuEnabled
    {
        #region PrivateMembers

        private readonly ICalculateService _calculateService;
        private readonly IContractService _contractService;
        private readonly IMessageBoxService _messageBoxService;
        private bool _isDelayed;
        private int _howManyDaysLate;
        private Business.Models.Contract _contract;
        private NotifyTask<IList<LendingRate>> _lendingRates;
        private LendingRate _selectedNewRepurchaseDateLendingRate;
        private LendingRate _selectedDelayLendingRate;
        private LendingRate _actualLendingRate;
        private DateTime _contractStartDate;
        private Func<Task> _callBack;

        #endregion

        #region Constructor

        public RenewContractDataViewModel(ICalculateService calculateService, IContractService contractService,
            IContainerProvider containerProvider, IMessageBoxService messageBoxService)
        {
            _calculateService = calculateService;
            _contractService = contractService;
            _messageBoxService = messageBoxService;
            Contract = new Business.Models.Contract { ContractItems = new List<ContractItem>(), LendingRate = new LendingRate() };
            HamburgerMenuItem = containerProvider.Resolve<RenewContractPaymentHamburgerMenuItem>();
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
                RaisePropertyChanged(nameof(RePurchasePrice));
            }
        }

        public DateTime ContractStartDate
        {
            get => _contractStartDate;
            set => SetProperty(ref _contractStartDate, value);
        }

        public DateTime ContractDate
        {
            get
            {
                if (Contract.ContractRenews.Count == 0)
                {
                    _actualLendingRate = Contract.LendingRate;
                    RaisePropertyChanged(nameof(RenewPrice));
                    RaisePropertyChanged(nameof(RePurchasePrice));
                    ContractStartDate = Contract.StartDate;
                    return Contract.StartDate.AddDays(Contract.LendingRate.Days);
                }

                var lastRenew = Contract.ContractRenews
                    .OrderByDescending(c => c.RenewContractId)
                    .First();
                _actualLendingRate = lastRenew.LendingRate;
                RaisePropertyChanged(nameof(RenewPrice));
                RaisePropertyChanged(nameof(RePurchasePrice));
                ContractStartDate = lastRenew.StartDate;
                return lastRenew.StartDate.AddDays(lastRenew.LendingRate.Days);
            }
        }

        public NotifyTask<int> HowManyDaysLateCalculated => NotifyTask.Create(GetHowManyDaysCalculated);

        public decimal SumOfEstimatedValues => Contract.ContractItems.Sum(c => c.EstimatedValue);

        public decimal RePurchasePrice =>
            _actualLendingRate is not null
                ? _calculateService.CalculateContractAmount(SumOfEstimatedValues, _actualLendingRate)
                : 0;

        public bool IsDelayed
        {
            get => _isDelayed;
            set => SetProperty(ref _isDelayed, value);
        }

        public DateTime? NewRepurchaseDate =>
            SelectedNewRepurchaseDateLendingRate is not null
                ? ContractDate.AddDays(SelectedNewRepurchaseDateLendingRate.Days)
                : null;

        public int HowManyDaysLate
        {
            get => _howManyDaysLate;
            set
            {
                SetProperty(ref _howManyDaysLate, value);
                RaisePropertyChanged(nameof(RenewPrice));
            }
        }

        public NotifyTask<decimal> RenewPrice => NotifyTask.Create(GetRenewPrice);

        public decimal NewRePurchasePrice => SelectedNewRepurchaseDateLendingRate is not null
            ? _calculateService.CalculateContractAmount(SumOfEstimatedValues,
                SelectedNewRepurchaseDateLendingRate)
            : 0;

        public NotifyTask<IList<LendingRate>> LendingRates
        {
            get => _lendingRates;
            set
            {
                SetProperty(ref _lendingRates, value);
                RaisePropertyChanged(nameof(RenewPrice));
            }
        }

        public LendingRate SelectedNewRepurchaseDateLendingRate
        {
            get => _selectedNewRepurchaseDateLendingRate;
            set
            {
                SetProperty(ref _selectedNewRepurchaseDateLendingRate, value);
                RaisePropertyChanged(nameof(NewRepurchaseDate));
                RaisePropertyChanged(nameof(NewRePurchasePrice));
                RaisePropertyChanged(nameof(IsNextButtonEnabled));
                (this as IHamburgerMenuEnabled).IsEnabled = IsNextButtonEnabled;
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

        public bool IsNextButtonEnabled => NewRepurchaseDate is not null;

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
            var days = DateTime.Compare(ContractDate, DateTime.Now) < 0
                ? DateTime.Today.Subtract(ContractDate).Days
                : 0;
            IsDelayed = days > 0;
            HowManyDaysLate = days;
            SelectedDelayLendingRate =
                (await LendingRates.Task)?.FirstOrDefault(lr => lr.Days == days) ?? new LendingRate { Days = days };
            return days;
        }

        private async Task<decimal> GetRenewPrice()
        {
            return _actualLendingRate is not null && LendingRates is not null
                  ? _calculateService.CalculateRenewCost(SumOfEstimatedValues, _actualLendingRate, HowManyDaysLate,
                     await LendingRates.Task)
                  : 0;
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
            navigationContext.Parameters.Add("renewPrice", RenewPrice.Result);
            navigationContext.Parameters.Add("renewLendingRate", SelectedNewRepurchaseDateLendingRate);
            navigationContext.Parameters.Add("startDate", ContractDate);
            navigationContext.Parameters.Add("CallBack", _callBack);
        }

        #endregion

        #region IHamburgerMenuEnabled

        public HamburgerMenuItemBase HamburgerMenuItem { get; set; }

        #endregion
    }
}