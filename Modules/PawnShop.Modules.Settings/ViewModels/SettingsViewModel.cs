using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace PawnShop.Modules.Settings.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public SettingsViewModel(IRegionManager regionManager, IContainerProvider containerProvider)
        {
            _regionManager = regionManager;

        }
    }
}
