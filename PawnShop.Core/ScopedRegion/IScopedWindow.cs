namespace PawnShop.Core.ScopedRegion
{
    public interface IScopedWindow
    {
        void RegisterViews(IContainerProvider containerProvider, IRegionManager scopedRegionManager);
    }
}
