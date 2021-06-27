using PawnShop.Controls.ContractItemViews.Validators;
using PawnShop.Core.ViewModel.Base;

namespace PawnShop.Controls.ContractItemViews.ViewModels
{
    public class LaptopViewModel : ViewModelBase<LaptopViewModel>
    {
        #region PrivateMembers

        private string _brand;
        private string _procesor;
        private string _ram;
        private string _driveType;
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


        public string Procesor
        {
            get => _procesor;
            set => SetProperty(ref _procesor, value);
        }

        public string Ram
        {
            get => _ram;
            set => SetProperty(ref _ram, value);
        }


        public string DriveType
        {
            get => _driveType;
            set => SetProperty(ref _driveType, value);
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