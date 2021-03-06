using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PawnShop.Core.Taskbar
{
    public class DependentViewRegionBehavior : RegionBehavior
    {
        #region private members

        private Dictionary<object, List<DependentViewInfo>> _dependentViewCache = new Dictionary<object, List<DependentViewInfo>>();

        #endregion private members

        #region public members

        public const string BehaviorKey = "DependentViewRegionBehavior";

        #endregion public members

        #region RegionBehavior implementantion

        protected override void OnAttach() => Region.ActiveViews.CollectionChanged += ActiveViewsCollectionChanged;

        #endregion RegionBehavior implementantion

        #region OnAttachEvent

        private void ActiveViewsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
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

                    dependentViewInfos.ForEach(dependentViewInfo => Region.RegionManager.Regions[dependentViewInfo.TaregetRegionName].Add(dependentViewInfo.View));
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldView in e.OldItems)
                {
                    if (_dependentViewCache.ContainsKey(oldView))
                    {
                        _dependentViewCache[oldView].ForEach(dependentViewInfo => Region.RegionManager.Regions[dependentViewInfo.TaregetRegionName].Remove(dependentViewInfo.View));

                        if (!ShouldKeepAlive(oldView))
                            _dependentViewCache.Remove(oldView);
                    }
                }
            }
        }

        #endregion OnAttachEvent

        #region helper methods

        private DependentViewInfo CreateDependentViewInfo(DependentViewAttribute att)
        {
            return new DependentViewInfo { TaregetRegionName = att.TargetRegionName, View = Activator.CreateInstance(att.Type) };
        }

        private IEnumerable<T> GetCustomAttributes<T>(Type type) => type.GetCustomAttributes(typeof(T), true).OfType<T>();

        private bool ShouldKeepAlive(object oldView)
        {
            return true;
        }

        #endregion helper methods
    }
}