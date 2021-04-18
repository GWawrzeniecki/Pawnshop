using PawnShop.Core.ScopedRegion;
using Prism.Mvvm;
using Prism.Regions;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class ContractDataViewModel : BindableBase, IRegionManagerAware
    {
        #region IRegionManagerAware
        public IRegionManager RegionManager { get; set; }
        #endregion
    }
}