using PawnShop.Core;
using PawnShop.Core.Dialogs;
using PawnShop.Core.Regions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Windows;

namespace PawnShop.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region private members

        private string _title = "Prism Application";
        private DelegateCommand<string> _navigateCommand;
        private readonly IRegionManager _regionManager;


        #endregion

        #region public properties
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }



        public DelegateCommand<string> NavigateCommand =>
_navigateCommand ??= new DelegateCommand<string>(ExecuteNavigateCommand);





        #endregion

        #region constructors

        public MainWindowViewModel(IRegionManager regionManager, IApllicationCommands applicationCommands)
        {
            applicationCommands.NavigateCommand.RegisterCommand(NavigateCommand);
            this._regionManager = regionManager;

        }

        #endregion


        #region private methods
        private void ExecuteNavigateCommand(string navigationPath)
        {
            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentException($"'{nameof(navigationPath)}' cannot be null or empty.", nameof(navigationPath));


            _regionManager.RequestNavigate(RegionNames.ContentRegion, navigationPath);
        }





        #endregion
    }
}
