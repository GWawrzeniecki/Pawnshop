using PawnShop.Modules.Commodity.RegionContext;
using Prism.Common;
using System.ComponentModel;
using System.Windows.Controls;

namespace PawnShop.Modules.Commodity.Base
{
    public abstract class GoodsViewBase : UserControl
    {
        private CommodityTabRegionContext _commodityTabRegionContext;

        protected GoodsViewBase()
        {
            ObserveCommodityTabRegionContext();
        }

        private void ObserveCommodityTabRegionContext()
        {
            ObservableObject<object> viewRegionContext = Prism.Regions.RegionContext.GetObservableContext(this);
            viewRegionContext.PropertyChanged += ViewRegionContext_OnPropertyChangedEvent;
        }

        private void ViewRegionContext_OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;
            if (context is null)
                return;
            var commodityTabRegionContext = context.Value as CommodityTabRegionContext;
            _commodityTabRegionContext = commodityTabRegionContext;
            PassRegionContextToDataContext();
        }

        private void PassRegionContextToDataContext()
        {
            if (DataContext is GoodsBaseViewModel goodsBaseViewModel)
                goodsBaseViewModel.CommodityTabRegionContext = _commodityTabRegionContext;
        }
    }
}