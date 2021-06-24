using BespokeFusion;
using PawnShop.Core;
using PawnShop.Core.Constants;
using PawnShop.Core.Regions;
using PawnShop.Core.SharedVariables;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Windows;

namespace PawnShop.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region private members

        private string _title = "Lombard \"VIP\"";
        private DelegateCommand<string> _navigateCommand;
        private readonly IRegionManager _regionManager;
        private readonly IConfigData _configData;
        private readonly IConfigurationService _configurationService;
        private bool _isPaneOpen;

        #endregion private members

        #region public properties

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => SetProperty(ref _isPaneOpen, value);
        }

        public DelegateCommand<string> NavigateCommand => _navigateCommand ??= new DelegateCommand<string>(ExecuteNavigateCommand);

        #endregion public properties

        #region constructors

        public MainWindowViewModel(IRegionManager regionManager, IApplicationCommands applicationCommands, IConfigData configData, IConfigurationService configurationService)
        {
            applicationCommands.NavigateCommand.RegisterCommand(NavigateCommand);
            this._regionManager = regionManager;
            _configData = configData;
            _configurationService = configurationService;
            LoadConfigData();
        }



        #endregion constructors

        #region private methods

        private void ExecuteNavigateCommand(string navigationPath)
        {
            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentException($"'{nameof(navigationPath)}' cannot be null or empty.", nameof(navigationPath));

            _regionManager.RequestNavigate(RegionNames.ContentRegion, navigationPath);
        }

        private void LoadConfigData()
        {
            try
            {
                _configData.VatPercent = _configurationService.GetValue<int>(Constants.VatPercentKey);
                _configData.DealDocumentPath = _configurationService.GetValue<string>(Constants.DealDocumentPath);
            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Nie udało się wczytać wartości z pliku konfiguracyjnego.{Environment.NewLine}Błąd: {e.Message}{Environment.NewLine}Aplikacja zostanie wyłączona.{Environment.NewLine}Skontaktuj się z administratorem.",
                    "Krytyczny błąd");
                Application.Current.Shutdown();
            }
        }

        #endregion private methods
    }
}