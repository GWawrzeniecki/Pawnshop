using Prism.Ioc;
using Prism.Regions;

namespace PawnShop.Core.ScopedRegion
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScopedWindow
    {
        void RegisterViews(IContainerProvider containerProvider, IRegionManager scopedRegionManager);
    }
}
