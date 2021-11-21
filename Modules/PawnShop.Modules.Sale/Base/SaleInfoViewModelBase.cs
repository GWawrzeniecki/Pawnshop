using PawnShop.Core.ScopedRegion;
using PawnShop.Core.ViewModel;
using Prism.Mvvm;
using Prism.Regions;

namespace PawnShop.Modules.Sale.Base
{
    public abstract class SaleInfoViewModelBase : BindableBase, ITabItemViewModel, IRegionManagerAware
    {
        #region PrivateMembers

        private Business.Models.Sale _sale;

        #endregion

        #region Constructor

        protected SaleInfoViewModelBase()
        {

        }

        #endregion

        #region PublicProperties

        public virtual Business.Models.Sale Sale
        {
            get => _sale;
            set
            {
                SetProperty(ref _sale, value);
                NavigateToChildViews();
            }
        }

        #endregion

        #region ITabItemViewModel

        public string Header { get; set; }

        #endregion

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion

        protected abstract void NavigateToChildViews();

    }
}