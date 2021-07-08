using Prism.Regions;

namespace PawnShop.Core.ScopedRegion
{
    public interface IRegionManagerAware
    {
        IRegionManager RegionManager { get; set; }
    }
}