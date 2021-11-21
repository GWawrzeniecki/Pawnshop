using PawnShop.Core.Regions;
using PawnShop.Modules.Sale.Base;
using Prism.Regions;

namespace PawnShop.Modules.Sale.ViewModels
{
    public class SaleBasicInfoViewModel : SaleInfoViewModelBase
    {
        #region PrivateMembers

        private Business.Models.Sale _sale;

        #endregion

        #region Constructor

        public SaleBasicInfoViewModel()
        {
            Header = "Informacje podstawowe";
        }

        #endregion

        #region PublicProperties

        public override Business.Models.Sale Sale
        {
            get => _sale;
            set
            {
                SetProperty(ref _sale, value);
                NavigateToChildViews();
            }
        }

        #endregion

        #region SaleInfoViewModelBase

        protected override void NavigateToChildViews()
        {
            RegionManager.RequestNavigate(RegionNames.ShowPreviewSaleBasicInfoRegion, nameof(Controls.SharedViews.Views.SaleBaseInfo), new NavigationParameters() { { "sale", Sale } });
            RegionManager.RequestNavigate(RegionNames.ShowPreviewSaleInfoRegion, nameof(Controls.SharedViews.Views.SaleInfo), new NavigationParameters() { { "sale", Sale } });
        }

        #endregion

    }
}
