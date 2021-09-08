using PawnShop.Core.ViewModel;
using Prism.Mvvm;

namespace PawnShop.Modules.Client.ViewModels
{
    public class DealTabViewModel : BindableBase, ITabItemViewModel
    {
        public DealTabViewModel()
        {
            Header = "Umowy";
        }

        #region ITabItemViewModel

        public string Header { get; set; }

        #endregion


    }
}
