using ControlzEx.Theming;
using PawnShop.Core.SharedVariables;
using PawnShop.Core.ViewModel;
using PawnShop.Modules.Settings.Models;
using PawnShop.Services.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PawnShop.Modules.Settings.ViewModels
{
    public class AppSettingsViewModel : BindableBase, ITabItemViewModel
    {
        #region PrivateMembers

        private AppTheme _selectedAppTheme;
        private IList<AppTheme> _appThemes;
        private bool _isAppThemeChangedByUser;
        private readonly ISettingsService<UserSettings> _userSettingsService;
        private readonly IUserSettings _userSettings;
        private readonly IMessageBoxService _messageBoxService;

        #endregion

        #region Constructor
        public AppSettingsViewModel(ISettingsService<UserSettings> userSettingsService, IUserSettings userSettings, IMessageBoxService messageBoxService)
        {
            _userSettingsService = userSettingsService;
            _userSettings = userSettings;
            _messageBoxService = messageBoxService;
            Header = "Aplikacja";
            LoadAppThemes();
            LoadActualAppTheme();
        }

        #endregion

        #region PublicProperties

        public string Header { get; set; }

        public IList<AppTheme> AppThemes
        {
            get => _appThemes;
            set => SetProperty(ref _appThemes, value);
        }

        public AppTheme SelectedAppTheme
        {
            get => _selectedAppTheme;
            set
            {
                SetProperty(ref _selectedAppTheme, value);
                if (_isAppThemeChangedByUser)
                {
                    ChangeTheme(value);
                    SaveCurrentTheme(value);
                }
            }
        }

        #endregion

        #region PrivateMethods

        private void LoadAppThemes()
        {
            AppThemes = new List<AppTheme>
            {
                new() {DisplayThemeName = "Jasny", ThemeName = "Light.Blue"},
                new() {DisplayThemeName = "Ciemny", ThemeName = "Dark.Steel"}
            };
        }

        private void LoadActualAppTheme()
        {
            var themeName = _userSettings.ThemeName;
            SelectedAppTheme = AppThemes.FirstOrDefault(appTheme => appTheme.ThemeName.Equals(themeName));
            _isAppThemeChangedByUser = true;
        }

        private void SaveCurrentTheme(AppTheme appTheme)
        {
            try
            {
                _userSettings.ThemeName = appTheme.ThemeName;
                _userSettingsService.SaveSettings(_userSettings as UserSettings);
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Wystąpił błąd podczas zapisywania motywu do ustawień. {Environment.NewLine}Błąd: {e.Message}", "Błąd");
            }
        }

        private static void ChangeTheme(AppTheme value)
        {
            _ = ThemeManager.Current.ChangeTheme(Application.Current, value.ThemeName);
        }


        #endregion
    }
}
