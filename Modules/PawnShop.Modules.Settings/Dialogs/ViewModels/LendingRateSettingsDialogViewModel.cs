using CloneExtensions;
using PawnShop.Business.Models;
using PawnShop.Core.Dialogs;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Settings.Validators;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawnShop.Modules.Settings.Dialogs.ViewModels
{
    public class LendingRateSettingsDialogViewModel : ViewModelBase<LendingRateSettingsDialogViewModel>, IDialogAware
    {
        #region PrivateMembers

        private readonly IContractService _contractService;
        private readonly ILendingRateService _lendingRateService;
        private readonly IDialogService _dialogService;
        private readonly IMessageBoxService _messageBoxService;
        private IList<LendingRate> _lendingRates;
        private DelegateCommand _addLendingRateCommand;
        private DelegateCommand _deleteLendingRateCommand;
        private DelegateCommand _editLendingRateCommand;
        private int? _days;
        private int? _percentage;
        private LendingRate _selectedLendingRate;
        private bool _hasDaysText;
        private bool _hasPercentageText;

        #endregion

        #region Constructor
        public LendingRateSettingsDialogViewModel(IContractService contractService, ILendingRateService lendingRateService, IDialogService dialogService, LendingRateSettingsDialogValidator addEditLendingRateValidator, IMessageBoxService messageBoxService) : base(addEditLendingRateValidator)
        {
            _contractService = contractService;
            _lendingRateService = lendingRateService;
            _dialogService = dialogService;
            _messageBoxService = messageBoxService;
            LoadStartupDataAsync();
        }

        #endregion

        #region PublicProperties

        public string Title => "Ustawienia oprocentowania";

#pragma warning disable CS0067 // The event 'LendingRateSettingsDialogViewModel.RequestClose' is never used
        public event Action<IDialogResult> RequestClose;
#pragma warning restore CS0067 // The event 'LendingRateSettingsDialogViewModel.RequestClose' is never used

        public IList<LendingRate> LendingRates
        {
            get => _lendingRates;
            set => SetProperty(ref _lendingRates, value);
        }

        public int? Days
        {
            get => _days;
            set => SetProperty(ref _days, value);
        }

        public int? Percentage
        {
            get => _percentage;
            set => SetProperty(ref _percentage, value);
        }

        public LendingRate SelectedLendingRate
        {
            get => _selectedLendingRate;
            set => SetProperty(ref _selectedLendingRate, value);
        }

        public bool HasDaysText
        {
            get => _hasDaysText;
            set => SetProperty(ref _hasDaysText, value);
        }
        public bool HasPercentageText
        {
            get => _hasPercentageText;
            set => SetProperty(ref _hasPercentageText, value);
        }


        #endregion

        #region Commands

        public DelegateCommand AddLendingRateCommand =>
            _addLendingRateCommand ??= new DelegateCommand(AddLendingRate, CanExecuteAddLendingRate)
                .ObservesProperty(() => HasDaysText)
                .ObservesProperty(() => HasPercentageText)
                .ObservesProperty(() => HasErrors);

        public DelegateCommand EditLendingRateCommand =>
            _editLendingRateCommand ??= new DelegateCommand(EditLendingRate, CanExecuteEditDelete)
                .ObservesProperty(() => SelectedLendingRate);


        public DelegateCommand DeleteLendingRateCommand =>
            _deleteLendingRateCommand ??=
                new DelegateCommand(DeleteLendingRateAsync, CanExecuteEditDelete)
                    .ObservesProperty(() => SelectedLendingRate);

        #endregion

        #region CommandsMethods

        private async void AddLendingRate()
        {
            try
            {
                await _lendingRateService.AddLendingRate(Days.Value, Percentage.Value);
                Days = null;
                Percentage = null;
                _messageBoxService.Show("Pomyślnie dodano oprocentowanie.", "Sukces");
                LoadStartupDataAsync();
            }
            catch (AddingLendingRateException addingLendingRateException)
            {
                _messageBoxService.ShowError(
                    $"{addingLendingRateException.Message}{Environment.NewLine}Błąd: {addingLendingRateException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private void EditLendingRate()
        {
            var initializers = new Dictionary<Type, Func<object, object>>
            {
                { typeof(ICollection<ContractRenew> ), s =>  new HashSet<ContractRenew>() },
                { typeof(ICollection<Contract>  ), s =>  new HashSet<Contract>() }
            };

            _dialogService.ShowEditLendingRateSettingsDialog(SelectedLendingRate.GetClone(initializers), result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    LoadStartupDataAsync();
                }
            });


        }

        private async void DeleteLendingRateAsync()
        {
            try
            {
                await _lendingRateService.DeleteLendingRate(SelectedLendingRate);
                _messageBoxService.Show("Pomyślnie usunięto oprocentowanie.", "Sukces");
                SelectedLendingRate = null;
                LoadStartupDataAsync();
            }
            catch (DeletingLendingRateException deletingLendingRateException)
            {
                _messageBoxService.ShowError(
                    $"{deletingLendingRateException.Message}{Environment.NewLine}Błąd: {deletingLendingRateException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private bool CanExecuteEditDelete()
        {
            return SelectedLendingRate is not null;
        }

        private bool CanExecuteAddLendingRate()
        {
            return HasDaysText && HasPercentageText && !HasErrors;
        }
        #endregion

        #region PrivateMethods

        private async void LoadStartupDataAsync()
        {
            try
            {
                await TryToLoadStartupDataAsync();
            }
            catch (LoadingLendingRatesException laodingLendingRateException)
            {
                _messageBoxService.ShowError(
                    $"{laodingLendingRateException.Message}{Environment.NewLine}Błąd: {laodingLendingRateException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private async Task TryToLoadStartupDataAsync()
        {

            LendingRates = await _contractService.LoadLendingRates();
        }

        #endregion

        #region IDialogAware

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        #endregion IDialogAware

        #region viewModelBase

        protected override LendingRateSettingsDialogViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase



    }
}