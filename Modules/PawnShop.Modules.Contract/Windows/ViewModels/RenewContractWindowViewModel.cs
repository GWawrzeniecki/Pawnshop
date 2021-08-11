using PawnShop.Modules.Contract.Windows.Views;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;

namespace PawnShop.Modules.Contract.Windows.ViewModels
{
    public class RenewContractWindowViewModel : BindableBase
    {

        #region PrivateMembers

        private bool _isBusy;
        private object _selectedItem;
        private string _tittle;
        private DelegateCommand<Type> _navigateCommand;
        private DelegateCommand _closeShellCommand;
        private readonly IContainerProvider _containerProvider;
        private readonly IShellService _shellService;

        #endregion

        #region Constructor

        public RenewContractWindowViewModel(IContainerProvider containerProvider, IShellService shellService)
        {
            _containerProvider = containerProvider;
            _shellService = shellService;
            Tittle = "Przedłużenie umowy";
        }

        #endregion

        #region Commands

        public DelegateCommand<Type> NavigateCommand =>
            _navigateCommand ??=
                new DelegateCommand<Type>(Navigate);

        public DelegateCommand CloseShellCommand =>
            _closeShellCommand ??= new DelegateCommand(CloseShell);



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


        public string Tittle
        {
            get => _tittle;
            set => SetProperty(ref _tittle, value);
        }

        #endregion

        #region commandMethods

        public void Navigate(Type type)
        {
            var hamburgerMenuItem = _containerProvider.Resolve(type);
            SelectedItem = hamburgerMenuItem;
        }

        private void CloseShell()
        {
            _shellService.CloseShell<RenewContractWindow>();
        }

        #endregion

    }
}
