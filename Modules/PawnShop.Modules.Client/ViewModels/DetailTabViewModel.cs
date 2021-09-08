using PawnShop.Core.ViewModel;
using Prism.Mvvm;

namespace PawnShop.Modules.Client.ViewModels
{
    public class DetailTabViewModel : BindableBase, ITabItemViewModel
    {
        public DetailTabViewModel()
        {
            Header = "Dane szczegółowe";
        }

        #region ITabItemViewModel

        public string Header { get; set; }

        #endregion
    }
}
