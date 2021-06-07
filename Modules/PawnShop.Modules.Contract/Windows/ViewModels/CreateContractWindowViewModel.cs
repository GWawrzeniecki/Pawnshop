using PawnShop.Core.HamburgerMenu.Implementations;
using PawnShop.Core.ScopedRegion;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Modules.Contract.Validators;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;

namespace PawnShop.Modules.Contract.Windows.ViewModels
{
    public class CreateContractWindowViewModel : ViewModelBase<CreateContractWindowViewModel>, IRegionManagerAware
    {
        #region constructor

        public CreateContractWindowViewModel(CreateContractValidator dialogValidator,
            IContainerProvider containerProvider) : base(dialogValidator)
        {
            _containerProvider = containerProvider;
            Tittle = "Rejestracja nowej umowy";

            //Task.Factory.StartNew(async () =>
            //{
            //    await Task.Delay(5000);
            //    var t = _containerProvider.Resolve<ContractDataHamburgerMenuItem>();
            //    SelectedItem = t;
            //}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
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

        #region private members

        private string _tittle;
        private object _selectedItem;
        private readonly IContainerProvider _containerProvider;
        private int _selectedIndex;
        private IList<HamburgerMenuItemBase> _hamburgerMenuItems;
        private DelegateCommand<Type> _navigateCommand;

        #endregion private members

        #region Commands

        public DelegateCommand<Type> NavigateCommand =>
            _navigateCommand ??=
                new DelegateCommand<Type>(Navigate);

        #endregion

        #region public Properties

        public IList<HamburgerMenuItemBase> HamburgerMenuItems
        {
            get => _hamburgerMenuItems;
            set => SetProperty(ref _hamburgerMenuItems, value);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        public object SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public string Tittle
        {
            get => _tittle;
            set => SetProperty(ref _tittle, value);
        }

        #endregion public Properties


        #region commandMethods

        public void Navigate(Type type)
        {
            var hamburgerMenuItem = _containerProvider.Resolve(type);
            SelectedItem = hamburgerMenuItem;
        }

        #endregion
    }
}