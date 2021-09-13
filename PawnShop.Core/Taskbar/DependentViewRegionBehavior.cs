using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace PawnShop.Core.Taskbar
{
    public class DependentViewRegionBehavior : RegionBehavior
    {
        #region private members

        private readonly Dictionary<object, List<DependentViewInfo>> _dependentViewCache = new();

        #endregion private members

        #region public members

        public const string BehaviorKey = "DependentViewRegionBehavior";

        #endregion public members

        #region RegionBehavior implementantion

        protected override void OnAttach() => Region.ActiveViews.CollectionChanged += ActiveViewsCollectionChanged;

        #endregion RegionBehavior implementantion

        #region OnAttachEvent

        private void ActiveViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var view in e.NewItems)
                {
                    var dependentViewInfos = new List<DependentViewInfo>();

                    if (_dependentViewCache.ContainsKey(view))
                    {
                        dependentViewInfos = _dependentViewCache[view];
                    }
                    else
                    {
                        foreach (var att in GetCustomAttributes<DependentViewAttribute>(view.GetType()))
                        {
                            var dependentViewInfo = CreateDependentViewInfo(att);

                            if (dependentViewInfo.View is ISupportDataContext dependentViewDataContext && view is ISupportDataContext viewDataContext)
                                dependentViewDataContext.DataContext = viewDataContext.DataContext;

                            dependentViewInfos.Add(dependentViewInfo);
                        }

                        if (!_dependentViewCache.ContainsKey(view))
                            _dependentViewCache.Add(view, dependentViewInfos);
                    }

                    dependentViewInfos.ForEach(dependentViewInfo => Region.RegionManager.Regions[dependentViewInfo.TargetRegionName].Add(dependentViewInfo.View));
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldView in e.OldItems)
                {
                    if (_dependentViewCache.ContainsKey(oldView))
                    {
                        _dependentViewCache[oldView].ForEach(dependentViewInfo => Region.RegionManager.Regions[dependentViewInfo.TargetRegionName].Remove(dependentViewInfo.View));

                        if (!ShouldKeepAlive(oldView))
                            _dependentViewCache.Remove(oldView);
                    }
                }
            }
        }

        #endregion OnAttachEvent

        #region helper methods

        private static DependentViewInfo CreateDependentViewInfo(DependentViewAttribute att)
        {
            return new() { TargetRegionName = att.TargetRegionName, View = Activator.CreateInstance(att.Type) };
        }

        private static IEnumerable<T> GetCustomAttributes<T>(Type type) => type.GetCustomAttributes(typeof(T), true).OfType<T>();

        private static bool ShouldKeepAlive(object oldView)
        {
            IRegionMemberLifetime regionMemberLifetime = GetItemOrContextLifetime(oldView);
            if (regionMemberLifetime != null)
                return regionMemberLifetime.KeepAlive;

            RegionMemberLifetimeAttribute regionMemberLifetimeAttribute = GetItemOrContentLifetimeAttribute(oldView);
            if (regionMemberLifetimeAttribute != null)
                return regionMemberLifetimeAttribute.KeepAlive;

            return true;
        }

        private static IRegionMemberLifetime GetItemOrContextLifetime(object oldView)
        {
            return oldView switch
            {
                IRegionMemberLifetime regionMemberLifetime => regionMemberLifetime,
                FrameworkElement frameworkElement => frameworkElement.DataContext as IRegionMemberLifetime,
                _ => null
            };
        }

        private static RegionMemberLifetimeAttribute GetItemOrContentLifetimeAttribute(object oldView)
        {
            var lifeAttribute = GetCustomAttributes<RegionMemberLifetimeAttribute>(oldView.GetType()).FirstOrDefault();
            if (lifeAttribute != null)
                return lifeAttribute;

            if (oldView is FrameworkElement frameworkElement && frameworkElement.DataContext != null)
                return GetCustomAttributes<RegionMemberLifetimeAttribute>(frameworkElement.DataContext.GetType()).FirstOrDefault();

            return null;
        }

        #endregion helper methods
    }
}