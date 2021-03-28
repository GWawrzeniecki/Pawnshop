using PawnShop.Core.ScopedRegion;
using Prism.Mvvm;
using Prism.Regions;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class ClientDataViewModel : BindableBase, IRegionManagerAware
    {
        #region constructor

        public ClientDataViewModel()
        {
        }

        #endregion constructor

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion IRegionManagerAware
    }
}