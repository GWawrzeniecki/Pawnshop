using PawnShop.Core.Enums;
using PawnShop.Core.Regions;
using PawnShop.Modules.Sale.Base;
using Prism.Regions;
using System;

namespace PawnShop.Modules.Sale.ViewModels
{
    public class SaleAdditionalInfoViewModel : SaleInfoViewModelBase
    {
        #region PrivateMembers

        private Business.Models.Sale _sale;
        private string _itemInfoGroupBoxHeaderName;

        #endregion

        public SaleAdditionalInfoViewModel()
        {
            Header = "Informacje dodatkowe";
        }

        #region PublicProperties

        public override Business.Models.Sale Sale
        {
            get => _sale;
            set
            {
                SetProperty(ref _sale, value);
                ItemInfoGroupBoxHeaderName = Sale.ContractItem.Category.Category;
                NavigateToChildViews();
            }
        }

        public string ItemInfoGroupBoxHeaderName
        {
            get => _itemInfoGroupBoxHeaderName;
            set => SetProperty(ref _itemInfoGroupBoxHeaderName, value);
        }

        #endregion

        #region SaleInfoViewModelBase

        protected override void NavigateToChildViews()
        {
            NavigateToShowPreviewSaleBasicItemInfoRegion(Sale);
            NavigateToShowPreviewAdditionalSaleInfoRegion(Sale);
        }

        #endregion

        #region PrivateMethods


        private void NavigateToShowPreviewSaleBasicItemInfoRegion(Business.Models.Sale sale)
        {
            switch (sale.ContractItem.Category.Category)
            {
                case Core.Constants.Constants.Laptop:
                    RegionManager.RequestNavigate(RegionNames.ShowPreviewSaleBasicItemInfoRegion, Core.Constants.Constants.Laptop, new NavigationParameters { { "dialogMode", DialogMode.ReadOnly }, { "laptop", sale.ContractItem.Laptop } });
                    break;
                default:
                    throw new NotImplementedException(sale.ContractItem.Category.Category);
            }
        }
        private void NavigateToShowPreviewAdditionalSaleInfoRegion(Business.Models.Sale sale)
        {
            RegionManager.RequestNavigate(RegionNames.ShowPreviewAdditionalSaleInfoRegion, nameof(Controls.SharedViews.Views.SaleInfo), new NavigationParameters() { { "sale", sale } });
        }

        #endregion
    }
}
