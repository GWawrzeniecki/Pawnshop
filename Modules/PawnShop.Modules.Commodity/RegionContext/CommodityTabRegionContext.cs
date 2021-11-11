using Prism.Mvvm;

namespace PawnShop.Modules.Commodity.RegionContext
{
    public class CommodityTabRegionContext : BindableBase
    {
        private bool _canExecute;

        public bool CanExecute
        {
            get => _canExecute;
            set => SetProperty(ref _canExecute, value);
        }
    }
}
