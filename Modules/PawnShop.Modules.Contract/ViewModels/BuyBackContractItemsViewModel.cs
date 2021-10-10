using PawnShop.Core.ScopedRegion;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class BuyBackContractItemsViewModel : BindableBase, IRegionManagerAware, INavigationAware
    {
        #region PrivateMembers

        private Business.Models.Contract _contractToBuyBack;
        private decimal _buyBackPrice;
        private object _callBack;

        #endregion

        #region Constructor

        #endregion

        #region PublicProperties



        #endregion

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        public Business.Models.Contract ContractToBuyBack
        {
            get => _contractToBuyBack;
            set => SetProperty(ref _contractToBuyBack, value);
        }

        #endregion

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var contract = navigationContext.Parameters.GetValue<Business.Models.Contract>("contract");
            if (contract is not null)
                ContractToBuyBack = contract;
            var buyBackPrice = navigationContext.Parameters.GetValue<decimal>("buyBackPrice");
            if (buyBackPrice != default)
                _buyBackPrice = buyBackPrice;
            _callBack = navigationContext.Parameters.GetValue<Func<Task>>("CallBack");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add("contract", ContractToBuyBack);
            navigationContext.Parameters.Add("buyBackPrice", _buyBackPrice);
            navigationContext.Parameters.Add("CallBack", _callBack);
        }

        #endregion
    }
}

