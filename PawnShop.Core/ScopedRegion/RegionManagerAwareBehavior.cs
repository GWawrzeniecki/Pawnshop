using System;
using System.Collections.Specialized;
using System.Windows;
using Prism.Regions;
using Region = Prism.Regions.Region;
using RegionBehavior = Prism.Regions.RegionBehavior;

namespace PawnShop.Core.ScopedRegion
{
    public class RegionManagerAwareBehavior : RegionBehavior
    {
        public const string BehaviorKey = "RegionManagerAwareBehavior";

        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
        }

        private void ActiveViews_CollectionChanged(object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    IRegionManager regionManager = Region.RegionManager;

                    if (item is FrameworkElement element)
                    {
                        if (element.GetValue(RegionManager.RegionManagerProperty) is IRegionManager scopedRegionManager)
                            regionManager = scopedRegionManager;
                    }

                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = regionManager);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    InvokeOnRegionManagerAwareElement(item, x => x.RegionManager = null);
                }
            }
        }

        private static void InvokeOnRegionManagerAwareElement(object item, Action<IRegionManagerAware> invocation)
        {
            switch (item)
            {
                case IRegionManagerAware rmAwareItem:
                    invocation(rmAwareItem);
                    break;

                case FrameworkElement frameworkElement:
                    {
                        if (frameworkElement.DataContext is IRegionManagerAware rmAwareDataContext)
                        {
                            if (frameworkElement.Parent is FrameworkElement frameworkElementParent)
                            {
                                if (frameworkElementParent.DataContext is IRegionManagerAware rmAwareDataContextParent)
                                {
                                    if (rmAwareDataContext == rmAwareDataContextParent)
                                    {
                                        return;
                                    }
                                }
                            }

                            invocation(rmAwareDataContext);
                        }

                        break;
                    }
            }
        }
    }
}