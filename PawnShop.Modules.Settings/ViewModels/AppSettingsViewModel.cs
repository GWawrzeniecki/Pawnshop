using BespokeFusion;
using ControlzEx.Theming;
using PawnShop.Core.Constants;
using PawnShop.Core.Services.Interfaces;
using PawnShop.Core.ViewModel;
using PawnShop.Modules.Settings.Models;
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
        private readonly IUserSettingsService _userSettingsService;
        private bool _isAppThemeChangedByUser;

        #endregion

        #region Constructor
        public AppSettingsViewModel(IUserSettingsService userSettingsService)
        {
            _userSettingsService = userSettingsService;
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
                    ThemeManager.Current.ChangeTheme(Application.Current, value.ThemeName);
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
                new() {DisplayThemeName = "Ciemny", ThemeName = "Dark.Blue"}
            };
        }

        private void LoadActualAppTheme()
        {
            var themeName = _userSettingsService.GetValue<string>(Constants.ThemeNameSection);
            SelectedAppTheme = AppThemes.FirstOrDefault(appTheme => appTheme.ThemeName.Equals(themeName));
            _isAppThemeChangedByUser = true;
        }

        private void SaveCurrentTheme(AppTheme appTheme)
        {
            try
            {
                _userSettingsService.SaveValue(Constants.ThemeNameSection, appTheme.ThemeName);
            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Wystąpił błąd podczas zapisywania motywu do ustawień. {Environment.NewLine}Błąd: {e.Message}", "Błąd");
            }
        }

        #endregion





    }
}
