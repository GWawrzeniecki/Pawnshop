using PawnShop.Controls.ContractItemViews.Validators;
using PawnShop.Core.ViewModel.Base;

namespace PawnShop.Controls.ContractItemViews.ViewModels
{
    public class LaptopViewModel : ViewModelBase<LaptopViewModel>
    {
        #region PrivateMembers

        private string _brand;
        private string _processor;
        private string _ram;
        private string _driverType;
        private string _massStorage;
        private string _descriptionKit;
        #endregion


        #region constructor 

        public LaptopViewModel(LaptopValidator laptopValidator) : base(laptopValidator)
        {


        }

        #endregion

        #region public properties

        public string Brand
        {
            get => _brand;
            set => SetProperty(ref _brand, value);
        }


        public string Processor
        {
            get => _processor;
            set => SetProperty(ref _processor, value);
        }

        public string Ram
        {
            get => _ram;
            set => SetProperty(ref _ram, value);
        }


        public string DriverType
        {
            get => _driverType;
            set => SetProperty(ref _driverType, value);
        }


        public string MassStorage
        {
            get => _massStorage;
            set => SetProperty(ref _massStorage, value);
        }


        public string DescriptionKit
        {
            get { return _descriptionKit; }
            set { SetProperty(ref _descriptionKit, value); }
        }


        #endregion

        #region viewModelBase

        protected override LaptopViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase


    }
}