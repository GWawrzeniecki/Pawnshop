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
using System.Threading.Tasks;

namespace PawnShop.Modules.Settings.ViewModels
{
    public class PawnShopSettingsViewModel : ViewModelBase<PawnShopSettingsViewModel>, ITabItemViewModel, IActiveAware
    {

        #region PrivateMembers

        private DelegateCommand _saveVatPercentCommand;
        private IUserSettings _userSettings;
        private readonly ISettingsService<UserSettings> _settingsService;
        private readonly IPdfService _pdfService;
        private readonly IDialogService _dialogService;
        private readonly IContainerProvider _containerProvider;
        private readonly IMessageBoxService _messageBoxService;
        private DelegateCommand _chooseDealDocumentFilePathCommand;
        private int _vatPercent;
        private string _dealDocumentPath;
        private DelegateCommand _setLendingRatesCommand;
        private bool _isActive;

        #endregion

        #region Constructor

        public PawnShopSettingsViewModel(IUserSettings userSettings, ISettingsService<UserSettings> settingsService, IPdfService pdfService,
            IDialogService dialogService, IContainerProvider containerProvider, PawnShopSettingsValidator pawnShopSettingsValidator, IMessageBoxService messageBoxService) : base(pawnShopSettingsValidator)
        {
            UserSettings = userSettings;
            _settingsService = settingsService;
            _pdfService = pdfService;
            _dialogService = dialogService;
            _containerProvider = containerProvider;
            _messageBoxService = messageBoxService;
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
                VatPercent = value.VatPercent;
                RaisePropertyChanged(nameof(DealDocumentPath));
                RaisePropertyChanged(nameof(VatPercent));
            }
        }

        public string Header { get; set; }

        //public string LendingRate => "";

        public int VatPercent
        {
            get => _vatPercent;
            set => SetProperty(ref _vatPercent, value);
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

        public DelegateCommand SaveVatPercentCommand =>
            _saveVatPercentCommand ??=
                new DelegateCommand(SaveVatPercent, CanExecuteSaveVatPercentCommand)
                    .ObservesProperty(() => HasErrors);

        public DelegateCommand ChooseDealDocumentFilePathCommand =>
            _chooseDealDocumentFilePathCommand ??= new DelegateCommand(ChooseDealDocumentTemplateFilePath);

        public DelegateCommand SetLendingRatesCommand =>
            _setLendingRatesCommand ??= new DelegateCommand(SetLendingRates);

        #endregion

        #region CommandMethods

        private void SaveVatPercent()
        {
            UserSettings.VatPercent = VatPercent;
            SaveUserSettings();
        }

        private async void ChooseDealDocumentTemplateFilePath()
        {

            if (await ChooseDealDocumentTemplateAsync())
                SaveUserSettings();
        }

        private void SetLendingRates()
        {
            _dialogService.ShowLendingRateSettingsDialog(null);
        }

        private bool CanExecuteSaveVatPercentCommand()
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
                _messageBoxService.Show("Zapisano pomyślnie", "Sukces");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Wystąpił błąd podczas zapisu ustawień. {Environment.NewLine}Błąd: {e.Message}", "Błąd");
            }
        }

        private void TryToSaveUserSettings()
        {
            _settingsService.SaveSettings(UserSettings as UserSettings);
        }

        private async Task<bool> ChooseDealDocumentTemplateAsync()
        {
            try
            {
                return await TryToChooseDealDocumentTemplateAsync();
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Wystąpił błąd podczas wybierania szablonu umowy. {Environment.NewLine}Błąd: {e.Message}", "Błąd");
                return false;
            }
        }

        private async Task<bool> TryToChooseDealDocumentTemplateAsync()
        {
            OpenFileDialog openFileDialog = new() { Filter = "Pdf files (*.pdf)|*.pdf" };
            if (openFileDialog.ShowDialog() != true) return false;

            var exist = await _pdfService.CheckIfPdfIsFillAbleAsync(openFileDialog.FileName);

            if (!exist)
                throw new Exception("Wybrany plik pdf nie posiada żadnych pól do wypełnienia.");

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
