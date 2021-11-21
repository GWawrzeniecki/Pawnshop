using AutoMapper;
using PawnShop.Controls.ContractItemViews.Validators;
using PawnShop.Core.Enums;
using PawnShop.Core.ViewModel.Base;
using Prism.Regions;

namespace PawnShop.Controls.ContractItemViews.ViewModels
{
    public class LaptopViewModel : ViewModelBase<LaptopViewModel>, INavigationAware
    {
        #region PrivateMembers

        private string _brand;
        private string _procesor;
        private string _ram;
        private string _driveType;
        private string _massStorage;
        private string _descriptionKit;
        private readonly IMapper _mapper;
        private bool _isViewReadOnly;

        #endregion

        #region constructor

        public LaptopViewModel(IMapper mapper, LaptopValidator laptopValidator) : base(laptopValidator)
        {
            _mapper = mapper;
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
            get => _descriptionKit;
            set => SetProperty(ref _descriptionKit, value);
        }

        public bool IsViewReadOnly
        {
            get => _isViewReadOnly;
            set => SetProperty(ref _isViewReadOnly, value);
        }

        #endregion

        #region viewModelBase

        protected override LaptopViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var laptopHasValue = navigationContext.Parameters.TryGetValue<Business.Models.Laptop>("laptop", out var laptop);
            if (laptopHasValue)
                _mapper.Map(laptop, this);
            var dialogModeHasValue = navigationContext.Parameters.TryGetValue<DialogMode>("dialogMode", out var dialogMode);
            if (dialogModeHasValue)
                IsViewReadOnly = dialogMode == DialogMode.ReadOnly;

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        #endregion

    }
}