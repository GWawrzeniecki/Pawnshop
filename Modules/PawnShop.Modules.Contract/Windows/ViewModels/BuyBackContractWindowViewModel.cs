using PawnShop.Modules.Contract.Windows.Views;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;

namespace PawnShop.Modules.Contract.Windows.ViewModels
{
    public class BuyBackContractWindowViewModel : BindableBase
    {
        private readonly IContainerProvider _containerProvider;
        private readonly IShellService _shellService;

        #region PrivateMembers

        private string _title;
        private bool _isBusy;
        private DelegateCommand<Type> _navigateCommand;
        private DelegateCommand _closeShellCommand;
        private object _selectedItem;

        #endregion

        #region Constructor

        public BuyBackContractWindowViewModel(IContainerProvider containerProvider, IShellService shellService)
        {
            _containerProvider = containerProvider;
            _shellService = shellService;
            Title = "Wykupienie umowy";
        }

        #endregion

        #region PublicProperties

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public object SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        #endregion

        #region Commands

        public DelegateCommand<Type> NavigateCommand =>
            _navigateCommand ??=
                new DelegateCommand<Type>(Navigate);

        public DelegateCommand CloseShellCommand =>
            _closeShellCommand ??= new DelegateCommand(CloseShell);

        #endregion

        #region CommandMethods
        public void Navigate(Type type)
        {
            var hamburgerMenuItem = _containerProvider.Resolve(type);
            SelectedItem = hamburgerMenuItem;
        }

        private void CloseShell()
        {
            _shellService.CloseShell<BuyBackContractWindow>();
        }

        #endregion

    }
}
