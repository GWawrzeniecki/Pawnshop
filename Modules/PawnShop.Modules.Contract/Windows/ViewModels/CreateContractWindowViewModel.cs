using PawnShop.Core;
using PawnShop.Core.ScopedRegion;
using PawnShop.Modules.Contract.Validators;
using Prism.Regions;

namespace PawnShop.Modules.Contract.Windows.ViewModels
{
    public class CreateContractWindowViewModel : ViewModelBase<CreateContractWindowViewModel>, IRegionManagerAware
    {
        #region constructor

        public CreateContractWindowViewModel(CreateContractValidator dialogValidator) : base(dialogValidator)
        {
            Tittle = "Rejestracja nowej umowy";
        }

        #endregion constructor

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion IRegionManagerAware

        #region viewModelBase

        protected override CreateContractWindowViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase

        #region public Properties

        private string _tittle;

        public string Tittle
        {
            get => _tittle;
            set => SetProperty(ref _tittle, value);
        }

        #endregion public Properties
    }
}