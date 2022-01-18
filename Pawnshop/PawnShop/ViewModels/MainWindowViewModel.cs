using AutoMapper;
using ControlzEx.Theming;
using Pawnshop.Setup.Scripts;
using PawnShop.Core;
using PawnShop.Core.Constants;
using PawnShop.Core.HamburgerMenu.Implementations;
using PawnShop.Core.Regions;
using PawnShop.Core.SharedVariables;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Home.MenuItem;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.IO;
using System.Reflection;
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
        private readonly IContainerProvider _containerProvider;
        private readonly IMessageBoxService _messageBoxService;
        private readonly ISetup _setup;
        private readonly IConfigurationService _configurationService;
        private bool _isPaneOpen;
        private DelegateCommand<string> _setSelectedMenuItemCommand;
        private ModuleHamburgerMenuItemBase _selectedItem;

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

        public ModuleHamburgerMenuItemBase SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        #endregion public properties

        #region Commands

        public DelegateCommand<string> NavigateCommand => _navigateCommand ??= new DelegateCommand<string>(ExecuteNavigateCommand);

        public DelegateCommand<string> SetSelectedMenuItemCommand =>
            _setSelectedMenuItemCommand ??= new DelegateCommand<string>(SetSelectedMenuItem);
        #endregion

        #region constructors

        public MainWindowViewModel(IRegionManager regionManager, IApplicationCommands applicationCommands, IUserSettings userSettings
        , ISettingsService<UserSettings> userSettingsService, IMapper mapper, IContainerProvider containerProvider, IMessageBoxService messageBoxService, ISetup setup
        , IConfigurationService configurationService)
        {
            applicationCommands.NavigateCommand.RegisterCommand(NavigateCommand);
            applicationCommands.SetMenuItemCommand.RegisterCommand(SetSelectedMenuItemCommand);
            _regionManager = regionManager;
            _userSettings = userSettings;
            _userSettingsService = userSettingsService;
            _mapper = mapper;
            _containerProvider = containerProvider;
            _messageBoxService = messageBoxService;
            _setup = setup;
            _configurationService = configurationService;
            ConfigureApplication(); // Because custom action in installer doesn't work ...
            LoadUserSettings();
            SetTheme();
        }

        #endregion constructors

        #region CommandMethods
        private void ExecuteNavigateCommand(string navigationPath)
        {
            if (string.IsNullOrEmpty(navigationPath))
                throw new ArgumentException($"'{nameof(navigationPath)}' cannot be null or empty.", nameof(navigationPath));

            _regionManager.RequestNavigate(RegionNames.ContentRegion, navigationPath);
        }

        private void SetSelectedMenuItem(string navigationPath)
        {
            SelectedItem = navigationPath switch
            {
                "Contract" => _containerProvider.Resolve<ContractHamburgerMenuItem>(),
                "Home" => _containerProvider.Resolve<HomeHamburgerMenuItem>(),
                _ => throw new NotImplementedException(navigationPath)
            };
        }

        #endregion

        #region private methods

        private void ConfigureApplication()
        {
            try
            {
                if (IsFirstLaunch()) return;
                _setup.ConfigureApplication(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                SetFirstLaunchToTrue();
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError($"Wystąpił błąd w konfiguracji aplikacji.{Environment.NewLine}Błąd: {e.Message}{Environment.NewLine}Aplikacja zostanie wyłączona.{Environment.NewLine}Skontaktuj się z administratorem.", "Błąd");
                Application.Current.Shutdown();
            }
        }

        private bool IsFirstLaunch()
        {
            return _configurationService.GetValueFromAppConfig<bool>(Constants.IsFirstLaunchAppConfigKeyName);
        }

        private void SetFirstLaunchToTrue()
        {
            _configurationService.SaveValueInAppConfig(Constants.IsFirstLaunchAppConfigKeyName, "True");
        }


        private void LoadUserSettings()
        {
            try
            {
                _mapper.Map(_userSettingsService.LoadSettings(), _userSettings);
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Nie udało się wczytać ustawień użytkownika.{Environment.NewLine}Błąd: {e.Message}{Environment.NewLine}Aplikacja zostanie wyłączona.{Environment.NewLine}Skontaktuj się z administratorem.",
                    "Krytyczny błąd");
                Application.Current.Shutdown();
            }
        }

        private void SetTheme()
        {
            if (_userSettings is null)
                return;

            if (!ThemeManager.Current.DetectTheme(Application.Current).Name.Equals(_userSettings.ThemeName))
                _ = ThemeManager.Current.ChangeTheme(Application.Current, _userSettings.ThemeName);
        }



        #endregion private methods
    }
}