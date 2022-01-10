using MahApps.Metro.Controls;
using Prism.Common;
using System.ComponentModel;
using System.Windows.Controls;

namespace PawnShop.Modules.Sale.Base
{
    public abstract class SaleInfoViewBase : UserControl
    {
        protected SaleInfoViewBase()
        {
            ObservePreviewSaleTabControlRegion();
        }

        private void ObservePreviewSaleTabControlRegion()
        {
            ObservableObject<object> viewRegionContext = Prism.Regions.RegionContext.GetObservableContext(this);
            viewRegionContext.PropertyChanged += ViewRegionContext_OnPropertyChangedEvent;
        }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        private void ViewRegionContext_OnPropertyChangedEvent(object? sender, PropertyChangedEventArgs e)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            var context = (ObservableObject<object>)sender;
            if (context is null)
                return;
            var sale = context.Value as Business.Models.Sale;
            PassRegionContextToDataContext(sale);
        }

        private void PassRegionContextToDataContext(Business.Models.Sale sale)
        {
            if (DataContext is SaleInfoViewModelBase saleInfoViewModelBase)
                saleInfoViewModelBase.Sale = sale;
        }

        protected void AdjustGrids(ContentControl firstContentControl, ContentControl secondContentControl, ContentControl SellGroupBox)
        {
            var firstGrid = firstContentControl.FindChild<Grid>("Grid");
            var secondGrid = secondContentControl.FindChild<Grid>("Grid");

            if (firstGrid is null || secondGrid is null)
                return;

            var width1 = firstGrid.ColumnDefinitions[0].ActualWidth;
            var width2 = secondGrid.ColumnDefinitions[0].ActualWidth;

            if (width1 > width2)
            {
                secondGrid.ColumnDefinitions[0].MinWidth = width1 - SellGroupBox.Padding.Left;
                secondGrid.ColumnDefinitions[0].MaxWidth = width1 - SellGroupBox.Padding.Left;
            }
            else
            {
                firstGrid.ColumnDefinitions[0].MinWidth = width2 + SellGroupBox.Padding.Left;
                firstGrid.ColumnDefinitions[0].MaxWidth = width2 + SellGroupBox.Padding.Left;
            }
        }
    }
}