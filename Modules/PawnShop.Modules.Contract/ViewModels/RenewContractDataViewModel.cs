using PawnShop.Core.ScopedRegion;
using PawnShop.Core.SharedVariables;
using PawnShop.Services.Interfaces;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Linq;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class RenewContractDataViewModel : BindableBase, IRegionManagerAware, INavigationAware
    {


        #region PrivateMembers
        private readonly ICalculateService _calculateService;



        private Business.Models.Contract _contract;
        #endregion


        #region Constructor

        public RenewContractDataViewModel(ICalculateService calculateService, ISessionContext sessionContext)
        {
            _calculateService = calculateService;
        
        }

        #endregion

        #region PublicProperties

        public Business.Models.Contract Contract
        {
            get => _contract;
            set => SetProperty(ref _contract, value);
        }


        public DateTime ContractDate
        {
            get
            {
                if (Contract.ContractRenews.Count == 0)
                    return Contract.StartDate.AddDays(Contract.LendingRate.Days);
                var lastRenew = Contract.ContractRenews
                    .OrderByDescending(c => c.RenewContractId)
                    .First();
                return lastRenew.StartDate.AddDays(lastRenew.LendingRate.Days);
            }
        }

        public int HowManyDaysLate => DateTime.Compare(ContractDate, DateTime.Now) < 0 ? DateTime.Today.Subtract(ContractDate).Days : 0;

        public decimal SumOfEstimatedValues => Contract.ContractItems.Sum(c => c.EstimatedValue);


        public decimal RePurchasePrice
        {
            get
            {
                if (Contract.ContractRenews.Count == 0)
                    return _calculateService.CalculateContractAmount(SumOfEstimatedValues, Contract.LendingRate);
                var lastRenew = Contract.ContractRenews
                    .OrderByDescending(c => c.RenewContractId)
                    .First();
                return _calculateService.CalculateContractAmount(SumOfEstimatedValues, lastRenew.LendingRate);
            }
        }


        #endregion

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion IRegionManagerAware

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            Contract = navigationContext.Parameters.GetValue<Business.Models.Contract>("contract");

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        #endregion

    }
}
