using PawnShop.Business.Models;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Settings.Validators;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;

namespace PawnShop.Modules.Settings.Dialogs.ViewModels
{
    public class EditLendingRateDialogViewModel : ViewModelBase<EditLendingRateDialogViewModel>, IDialogAware
    {
        #region PrivateMembers

        private readonly ILendingRateService _lendingRateService;
        private readonly IMessageBoxService _messageBoxService;
        private int? _days;
        private int? _percentage;
        private bool _hasDaysText;
        private bool _hasPercentageText;
        private DelegateCommand _editLendingRateCommand;
        private LendingRate _lendingRate;

        #endregion

        #region Constuctor
        public EditLendingRateDialogViewModel(ILendingRateService lendingRateService, EditLendingRateDialogValidator editLendingRateDialogValidator, IMessageBoxService messageBoxService) : base(editLendingRateDialogValidator)
        {
            _lendingRateService = lendingRateService;
            _messageBoxService = messageBoxService;
        }

        #endregion

        #region PublicProperties

        public string Title => "Edycja oprocentowania";

        public event Action<IDialogResult> RequestClose;

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

        public LendingRate LendingRate
        {
            get => _lendingRate;
            set => SetProperty(ref _lendingRate, value);
        }

        #endregion

        #region Commands

        public DelegateCommand EditLendingRateCommand =>
            _editLendingRateCommand ??= new DelegateCommand(EditLendingRateAsync, CanExecuteEditLendingRate)
                .ObservesProperty(() => HasDaysText)
                .ObservesProperty(() => HasPercentageText)
                .ObservesProperty(() => HasErrors);

        #endregion

        #region CommandsMethods

        private async void EditLendingRateAsync()
        {
            try
            {
                LendingRate.Days = Days.Value;
                LendingRate.Procent = Percentage.Value;             
                await _lendingRateService.EditLendingRate(LendingRate);
                _messageBoxService.Show("Pomyślnie zaktualizowano oprocentowanie.", "Sukces");
                RequestClose.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (EditingLendingRateException editingLendingRateException)
            {
                _messageBoxService.ShowError(
                    $"{editingLendingRateException.Message}{Environment.NewLine}Błąd: {editingLendingRateException.InnerException?.Message}",
                    "Błąd");

            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private bool CanExecuteEditLendingRate()
        {
            return HasDaysText && HasPercentageText && !HasErrors;
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
            LendingRate = parameters.GetValue<LendingRate>("lendingRate");
            Days = LendingRate.Days;
            Percentage = LendingRate.Procent;
        }

        #endregion IDialogAware

        #region viewModelBase

        protected override EditLendingRateDialogViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase
    }
}
