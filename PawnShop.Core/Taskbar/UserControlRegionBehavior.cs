using PawnShop.Core.Regions;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PawnShop.Core.Taskbar
{
    public class UserControlRegionBehavior : RegionBehavior
    {
        public const string BehaviorKey = "UserControlRegionBehavior";

        protected override void OnAttach()
        {
            if (Region.Name.Equals(RegionNames.ContentRegion))
                Region.ActiveViews.CollectionChanged += ActiveViewsCollectionChanged;
        }

        private void ActiveViewsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                var controlList = new List<UserControl>();
                foreach (var views in e.NewItems)
                {
                    foreach (var att in GetCustomAttributes<UserControlAttribute>(views.GetType()))
                    {
                        var userControl = Activator.CreateInstance(att.Type) as UserControl;
                        controlList.Add(userControl);
                    }
                    controlList.ForEach(userControl => Region.RegionManager.Regions[RegionNames.TopTaskBarRegion].Add(userControl));
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var views = Region.RegionManager.Regions[RegionNames.TopTaskBarRegion].Views.ToList();
                views.ForEach(view => Region.RegionManager.Regions[RegionNames.TopTaskBarRegion].Remove(view));
            }
        }

        private IEnumerable<T> GetCustomAttributes<T>(Type type) => type.GetCustomAttributes(typeof(T), true).OfType<T>();

    }
}
