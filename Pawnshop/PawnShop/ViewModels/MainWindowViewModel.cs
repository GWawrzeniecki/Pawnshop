using AutoMapper;
using BespokeFusion;
using ControlzEx.Theming;
using PawnShop.Core;
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
        private readonly IUserSettings _userSettings;
        private readonly ISettingsService<UserSettings> _userSettingsService;
        private readonly IMapper _mapper;
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

        public MainWindowViewModel(IRegionManager regionManager, IApplicationCommands applicationCommands, IUserSettings userSettings
        , ISettingsService<UserSettings> userSettingsService, IMapper mapper)
        {
            applicationCommands.NavigateCommand.RegisterCommand(NavigateCommand);
            _regionManager = regionManager;
            _userSettings = userSettings;
            _userSettingsService = userSettingsService;
            _mapper = mapper;
            LoadUserSettings();
            SetTheme();
        }

        #endregion constructors

        #region private methods

        private void ExecuteNavigateCommand(string navigationPath)
        {
            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentException($"'{nameof(navigationPath)}' cannot be null or empty.", nameof(navigationPath));

            _regionManager.RequestNavigate(RegionNames.ContentRegion, navigationPath);
        }

        private void LoadUserSettings()
        {
            try
            {
                if (!_userSettingsService.IsSettingsExist()) // until we don't have an installer
                {
                    _userSettingsService.SaveSettings(new UserSettings() { VatPercent = 23, ThemeName = "Light.Blue", DealDocumentsFolderPath = @"C:\Users\Kogut\Documents\PawnShop\DealDocuments", DealDocumentPath = @"C:\Users\Kogut\iCloudDrive\Documents\Inżynierka\Umowa\UMOWA KUPNA-SPRZEDAZY_V3-Form.pdf" });
                }

                _mapper.Map(_userSettingsService.LoadSettings(), _userSettings);
            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Nie udało się wczytać ustawień użytkownika.{Environment.NewLine}Błąd: {e.Message}{Environment.NewLine}Aplikacja zostanie wyłączona.{Environment.NewLine}Skontaktuj się z administratorem.",
                    "Krytyczny błąd");
                Application.Current.Shutdown();
            }
        }

        private void SetTheme()
        {
            if (!ThemeManager.Current.DetectTheme(Application.Current).Name.Equals(_userSettings.ThemeName))
                _ = ThemeManager.Current.ChangeTheme(Application.Current, _userSettings.ThemeName);
        }

        #endregion private methods
    }
}