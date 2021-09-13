using Prism.Regions;
using System;
using System.Collections.Specialized;
using System.Windows;

namespace PawnShop.Core.ScopedRegion
{
    /// <summary>
    /// Allows for scoped views obtains correct region manager in their view models
    /// </summary>
    public class RegionManagerAwareBehavior : RegionBehavior
    {
        public const string BehaviorKey = "RegionManagerAwareBehavior";

        protected override void OnAttach()
        {
            Region.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;
        }

        private void ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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
                            if (frameworkElement.Parent is FrameworkElement { DataContext: IRegionManagerAware rmAwareDataContextParent }) // if view doesn't have a view model it will inherit it from Parent
                            {
                                if (rmAwareDataContext == rmAwareDataContextParent)
                                {//view is using Parents view model 
                                    return;
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