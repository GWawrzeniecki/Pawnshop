using MahApps.Metro.Controls;
using PawnShop.Core.Events;
using PawnShop.Core.HamburgerMenu.Interfaces;
using PawnShop.Core.Models.ModuleVisibility;
using Prism.Events;
using System;
using IHamburgerMenuItemBase = MahApps.Metro.Controls.IHamburgerMenuItemBase;

namespace PawnShop.Core.HamburgerMenu.Implementations
{
    public abstract class ModuleHamburgerMenuItemBase : HamburgerMenuIconItem, IHamburgerMenuItemBase, IModuleVisibility
    {
        private readonly string _moduleName;
        private readonly IEventAggregator _eventAggregator;
        public abstract string DefaultNavigationPath { get; }

        protected ModuleHamburgerMenuItemBase(string moduleName, IEventAggregator eventAggregator)
        {
            _moduleName = moduleName;
            _eventAggregator = eventAggregator;
        }

        private void ModuleVisibilityChanged(ModuleVisibility moduleVisibility)
        {
            if (moduleVisibility.IsVisible)
                Show();
            else
                Hide();
        }

        public void Show()
        {
            IsVisible = true;
            IsEnabled = true;
        }

        public void Hide()
        {
            IsVisible = false;
            IsEnabled = false;
        }

        public void Subscribe()
        {
            IDisposable token = null;
            token = _eventAggregator.GetEvent<ModuleVisibilityEvent>().Subscribe((moduleVisibility) =>
            {
                ModuleVisibilityChanged(moduleVisibility);
                token.Dispose();

            }, ThreadOption.PublisherThread, true, vm => vm.ModuleName.Equals(_moduleName));
        }
    }
}