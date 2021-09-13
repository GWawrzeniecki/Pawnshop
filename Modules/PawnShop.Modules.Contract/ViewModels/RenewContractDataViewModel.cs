﻿using BespokeFusion;
using PawnShop.Business.Models;
using PawnShop.Core.HamburgerMenu.Implementations;
using PawnShop.Core.HamburgerMenu.Interfaces;
using PawnShop.Core.ScopedRegion;
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
        private bool _isDelayed;
        private int _howManyDaysLate;
        private Business.Models.Contract _contract;
        private IList<LendingRate> _lendingRates;
        private LendingRate _selectedNewRepurchaseDateLendingRate;
        private LendingRate _selectedDelayLendingRate;
        private LendingRate _actualLendingRate;
        private DateTime _contractStartDate;

        #endregion

        #region Constructor

        public RenewContractDataViewModel(ICalculateService calculateService, IContractService contractService,
            IContainerProvider containerProvider)
        {
            _calculateService = calculateService;
            _contractService = contractService;
            Contract = new Business.Models.Contract { ContractItems = new List<ContractItem>(), LendingRate = new LendingRate() };
            HamburgerMenuItem = containerProvider.Resolve<RenewContractPaymentHamburgerMenuItem>();
            LoadStartupData();
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
                ContractStartDate = Contract.StartDate;
                return lastRenew.StartDate.AddDays(lastRenew.LendingRate.Days);
            }
        }

        public int HowManyDaysLateCalculated
        {
            get
            {
                var days = DateTime.Compare(ContractDate, DateTime.Now) < 0
                    ? DateTime.Today.Subtract(ContractDate).Days
                    : 0;
                IsDelayed = days > 0;
                HowManyDaysLate = days;
                SelectedDelayLendingRate =
                    LendingRates?.FirstOrDefault(lr => lr.Days == days) ?? new LendingRate { Days = days };
                return days;
            }
        }

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

        public decimal RenewPrice => _actualLendingRate is not null && LendingRates is not null
            ? _calculateService.CalculateRenewCost(SumOfEstimatedValues, _actualLendingRate, HowManyDaysLate,
                LendingRates)
            : 0;

        public decimal NewRePurchasePrice => SelectedNewRepurchaseDateLendingRate is not null
            ? _calculateService.CalculateContractAmount(SumOfEstimatedValues,
                SelectedNewRepurchaseDateLendingRate)
            : 0;

        public IList<LendingRate> LendingRates
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

        private async void LoadStartupData()
        {
            try
            {
                await TryToLoadLendingRate();
            }

            catch (LoadingLendingRatesException loadingLendingRatesException)
            {
                MaterialMessageBox.ShowError(
                    $"{loadingLendingRatesException.Message}{Environment.NewLine}Błąd: {loadingLendingRatesException.InnerException?.Message}",
                    "Błąd");
            }
        }

        private async Task TryToLoadLendingRate()
        {
            LendingRates = await _contractService.LoadLendingRates();
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
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add("contract", Contract);
            navigationContext.Parameters.Add("renewPrice", RenewPrice);
            navigationContext.Parameters.Add("renewLendingRate", SelectedNewRepurchaseDateLendingRate);
            navigationContext.Parameters.Add("startDate", ContractDate);
        }

        #endregion

        #region IHamburgerMenuEnabled

        public HamburgerMenuItemBase HamburgerMenuItem { get; set; }

        #endregion
    }
}