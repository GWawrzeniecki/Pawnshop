using BespokeFusion;
using Microsoft.Win32;
using PawnShop.Core.Dialogs;
using PawnShop.Core.SharedVariables;
using PawnShop.Core.ViewModel;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Modules.Settings.Validators;
using PawnShop.Services.Interfaces;
using Prism;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;

namespace PawnShop.Modules.Settings.ViewModels
{
    public class PawnShopSettingsViewModel : ViewModelBase<PawnShopSettingsViewModel>, ITabItemViewModel, IActiveAware
    {

        #region PrivateMembers

        private DelegateCommand _saveSearchContractsDayCommand;
        private IUserSettings _userSettings;
        private readonly ISettingsService<UserSettings> _settingsService;
        private readonly IDialogService _dialogService;
        private readonly IContainerProvider _containerProvider;
        private DelegateCommand _chooseDealDocumentFilePathCommand;
        private int _automaticSearchingEndedContractsDay;
        private string _dealDocumentPath;
        private DelegateCommand _setLendingRatesCommand;
        private bool _isActive;

        #endregion

        #region Constructor

        public PawnShopSettingsViewModel(IUserSettings userSettings, ISettingsService<UserSettings> settingsService,
            IDialogService dialogService, IContainerProvider containerProvider, PawnShopSettingsValidator pawnShopSettingsValidator) : base(pawnShopSettingsValidator)
        {
            UserSettings = userSettings;
            _settingsService = settingsService;
            _dialogService = dialogService;
            _containerProvider = containerProvider;
            Header = "Lombard";
        }

        #endregion

        #region PublicProperties

        public IUserSettings UserSettings
        {
            get => _userSettings;
            set
            {
                _userSettings = value;
                DealDocumentPath = value.DealDocumentPath;
                AutomaticSearchingEndedContractsDay = value.AutomaticSearchingEndedContractsDay;
                RaisePropertyChanged(nameof(DealDocumentPath));
                RaisePropertyChanged(nameof(AutomaticSearchingEndedContractsDay));
            }
        }

        public string Header { get; set; }

        //public string LendingRate => "";

        public int AutomaticSearchingEndedContractsDay
        {
            get => _automaticSearchingEndedContractsDay;
            set => SetProperty(ref _automaticSearchingEndedContractsDay, value);
        }

        public string DealDocumentPath
        {
            get => _dealDocumentPath;
            set
            {
                SetProperty(ref _dealDocumentPath, value);
                UserSettings.DealDocumentPath = value;
            }
        }

        #endregion

        #region Commands

        public DelegateCommand SaveSearchContractsDayCommand =>
            _saveSearchContractsDayCommand ??=
                new DelegateCommand(SaveSearchContractsDay, CanExecuteSaveSearchContractDays)
                    .ObservesProperty(() => HasErrors);

        public DelegateCommand ChooseDealDocumentFilePathCommand =>
            _chooseDealDocumentFilePathCommand ??= new DelegateCommand(ChooseDealDocumentTemplateFilePath);

        public DelegateCommand SetLendingRatesCommand =>
            _setLendingRatesCommand ??= new DelegateCommand(SetLendingRates);

        #endregion

        #region CommandMethods

        private void SaveSearchContractsDay()
        {
            UserSettings.AutomaticSearchingEndedContractsDay = AutomaticSearchingEndedContractsDay;
            SaveUserSettings();
        }

        private void ChooseDealDocumentTemplateFilePath()
        {

            if (ChooseDealDocumentTemplate())
                SaveUserSettings();
        }

        private void SetLendingRates()
        {
            _dialogService.ShowLendingRateSettingsDialog(null);
        }

        private bool CanExecuteSaveSearchContractDays()
        {
            return !HasErrors;
        }

        #endregion

        #region PrivateMethods

        private void SaveUserSettings()
        {
            try
            {
                TryToSaveUserSettings();
                MaterialMessageBox.Show("Zapisano pomyślnie", "Sukces");
            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Wystąpił błąd podczas zapisu ustawień. {Environment.NewLine}Błąd: {e.Message}", "Błąd");
            }
        }

        private void TryToSaveUserSettings()
        {
            _settingsService.SaveSettings(UserSettings as UserSettings);
        }

        private bool ChooseDealDocumentTemplate()
        {
            try
            {
                return TryToChooseDealDocumentTemplate();
            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Wystąpił błąd podczas wybierania szablonu umowy. {Environment.NewLine}Błąd: {e.Message}", "Błąd");
                return false;
            }
        }

        private bool TryToChooseDealDocumentTemplate()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Pdf files (*.pdf)|*.pdf" };
            if (openFileDialog.ShowDialog() != true) return false;
            DealDocumentPath = openFileDialog.FileName;
            return true;

        }

        #endregion

        #region viewModelBase

        protected override PawnShopSettingsViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase

        #region IActiveAware


        public bool IsActive
        {
            get => _isActive;
            set
            {
                SetProperty(ref _isActive, value);
                if (!value)
                {
                    UserSettings = _containerProvider.Resolve<IUserSettings>();
                }
            }
        }
#pragma warning disable CS0067 // The event 'PawnShopSettingsViewModel.IsActiveChanged' is never used
        public event EventHandler IsActiveChanged;
#pragma warning restore CS0067 // The event 'PawnShopSettingsViewModel.IsActiveChanged' is never used



        #endregion

    }
}
