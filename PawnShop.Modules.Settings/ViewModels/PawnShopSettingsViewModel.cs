using PawnShop.Core.ViewModel;
using Prism.Mvvm;

namespace PawnShop.Modules.Settings.ViewModels
{
    public class PawnShopSettingsViewModel : BindableBase, ITabItemViewModel
    {
        public PawnShopSettingsViewModel()
        {
            Header = "Lombard";
            ;
        }

        public string Header { get; set; }
    }
}
