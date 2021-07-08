using Prism.Mvvm;

namespace PawnShop.Modules.Contract.Windows.ViewModels
{
    public class RenewContractWindowViewModel : BindableBase
    {

        #region PrivateMembers

        private bool _isBusy;
        private object _selectedItem;

        #endregion

        #region Constructor

        public RenewContractWindowViewModel()
        {

        }

        #endregion


        #region PublicProperties

        public object SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

    }
}
