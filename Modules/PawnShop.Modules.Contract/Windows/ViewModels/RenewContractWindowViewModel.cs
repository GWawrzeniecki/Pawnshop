using Prism.Mvvm;

namespace PawnShop.Modules.Contract.Windows.ViewModels
{
    public class RenewContractWindowViewModel : BindableBase
    {

        #region PrivateMembers

        private bool _isBusy;

        #endregion

        #region Constructor

        public RenewContractWindowViewModel()
        {

        }

        #endregion


        #region PublicProperties


        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        #endregion

    }
}
