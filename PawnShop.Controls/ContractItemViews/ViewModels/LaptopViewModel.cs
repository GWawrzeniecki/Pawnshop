using PawnShop.Core;
using Prism.Mvvm;

namespace PawnShop.Controls.ContractItemViews.ViewModels
{
    public class LaptopViewModel : BindableBase
    {
        #region PrivateMembers

        private string _brand;
        private string _processor;
        private string _ram;
        private string _driverType;
        private string _massStorage;
        #endregion

        public LaptopViewModel()
        {
        }

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


    }
}