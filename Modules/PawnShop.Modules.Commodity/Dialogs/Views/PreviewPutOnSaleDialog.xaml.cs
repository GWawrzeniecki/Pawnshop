﻿using MahApps.Metro.Controls;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using Prism.Common;
using Prism.Regions;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PawnShop.Modules.Commodity.Dialogs.Views
{
    /// <summary>
    /// Interaction logic for CommodityDialog.xaml
    /// </summary>
    public partial class PreviewPutOnSaleDialog : UserControl
    {
        private readonly IRegionManager _scopedRegionManager;
        private object _childView;

        public PreviewPutOnSaleDialog(IRegionManager regionManager)
        {
            InitializeComponent();
            _scopedRegionManager = regionManager.CreateRegionManager();
            RegionManager.SetRegionManager(ContentControl, _scopedRegionManager);
            RegionManagerAware.SetRegionManagerAware(this, _scopedRegionManager);
            _scopedRegionManager.Regions[RegionNames.PreviewPutOnSaleDialogContentRegion].Views.CollectionChanged += Views_CollectionChanged;
        }

        private void Views_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems is null)
                return;

            _childView = e.NewItems.OfType<DependencyObject>().First();
            ObservableObject<object> viewRegionContext = RegionContext.GetObservableContext((DependencyObject)_childView);
            viewRegionContext.PropertyChanged += this.ViewRegionContext_OnPropertyChangedEvent;
            _scopedRegionManager.Regions[RegionNames.PreviewPutOnSaleDialogContentRegion].Views.CollectionChanged -= Views_CollectionChanged;
        }

        private void ViewRegionContext_OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;

            if (context.Value is null)
                return;

            var hasValue = double.TryParse(context.Value.ToString(), out var width);

            if (hasValue && Grid.ColumnDefinitions[0].ActualWidth < width)
            {
                Grid.ColumnDefinitions[0].MinWidth = width;
                Grid.ColumnDefinitions[0].MaxWidth = width;
            }
            else
            {
                if (_childView is not FrameworkElement frameWorkElement)
                    return;

                var grid = frameWorkElement.FindChild<Grid>("Grid");

                if (grid is null)
                    return;

                grid.ColumnDefinitions[0].MinWidth = Grid.ColumnDefinitions[0].ActualWidth;
                grid.ColumnDefinitions[0].MaxWidth = Grid.ColumnDefinitions[0].ActualWidth;

            }
        }
    }
}
