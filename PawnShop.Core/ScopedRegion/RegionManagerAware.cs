using Prism.Regions;
using System.Windows;

namespace PawnShop.Core.ScopedRegion
{
    public class RegionManagerAware
    {
        /// <summary>
        /// Setting IRegionManager property from IRegionManagerAware Interface
        /// </summary>
        /// <param name="item"></param>
        /// <param name="regionManager"></param>
        public static void SetRegionManagerAware(object item, IRegionManager regionManager)
        {
            switch (item)
            {
                case IRegionManagerAware rmAware:
                    rmAware.RegionManager = regionManager;
                    break;

                case FrameworkElement rmAwareFrameworkElement:
                    {
                        if (rmAwareFrameworkElement.DataContext is IRegionManagerAware rmAwareDataContext)
                            rmAwareDataContext.RegionManager = regionManager;
                        break;
                    }
            }
        }
    }
}