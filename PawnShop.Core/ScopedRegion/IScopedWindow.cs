using Prism.Ioc;
using Prism.Regions;

namespace PawnShop.Core.ScopedRegion
{
    public interface IScopedWindow
    {
        void RegisterViews(IContainerProvider containerProvider, IRegionManager scopedRegionManager);
    }
}
