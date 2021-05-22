using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BespokeFusion;
using PawnShop.Business.Models;
using PawnShop.Core.Dialogs;
using PawnShop.Core.ScopedRegion;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.Services;
using PawnShop.Modules.Contract.Windows.Views;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class ContractDataViewModel : BindableBase, IRegionManagerAware
    {
        #region Private members

        private IList<LendingRate> _lendingRates;
        private readonly IContractService _contractService;
        private readonly IShellService _shellService;
        private readonly IDialogService _dialogService;
        private DelegateCommand _cancelCommand;
        private string _contractNumber;
        private LendingRate _lendingRate;
        private DateTime? _rePurchaseDateTime;
        private DelegateCommand _addContractItemCommand;
        private decimal _rePurchasePrice;
        private IList<ContractItem> _boughtContractItems;

        #endregion

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion

        #region PublicProperties

        public IList<LendingRate> LendingRates
        {
            get => _lendingRates;
            set => SetProperty(ref _lendingRates, value);
        }


        public string ContractNumber
        {
            get => _contractNumber;
            set => SetProperty(ref _contractNumber, value);
        }


        public LendingRate LendingRate
        {
            get => _lendingRate;
            set
            {
                SetProperty(ref _lendingRate, value);
                RepurchaseDate = value == null ? default : DateTime.Today.AddDays(value.Days);
            }
        }


        public DateTime? RepurchaseDate
        {
            get => _rePurchaseDateTime;
            set => SetProperty(ref _rePurchaseDateTime, value);
        }


        public decimal RePurchasePrice
        {
            get => _rePurchasePrice;
            set => SetProperty(ref _rePurchasePrice, value);
        }


        public IList<ContractItem> BoughtContractItems
        {
            get => _boughtContractItems;
            set => SetProperty(ref _boughtContractItems, value);
        }

        #endregion


        #region Commands

        public DelegateCommand CancelCommand =>
            _cancelCommand ??= new DelegateCommand(Cancel);

        public DelegateCommand AddContractItemCommand =>
            _addContractItemCommand ??= new DelegateCommand(AddContractItem);

        #endregion Commands

        #region constructor

        public ContractDataViewModel(IContractService contractService, IShellService shellService,
            IDialogService dialogService)
        {
            _contractService = contractService;
            _shellService = shellService;
            _dialogService = dialogService;
            BoughtContractItems = new List<ContractItem>();
            LoadStartupData();
        }

        #endregion

        #region CommandMethods

        private void Cancel()
        {
            _shellService.CloseShell<CreateContractWindow>();
        }

        private void AddContractItem()
        {
            _dialogService.ShowAddContractItemDialog(r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    BoughtContractItems.Add(r.Parameters.GetValue<ContractItem>("contractItem"));
               
                }
            });
        }

        #endregion

        #region private methods

        private async void LoadStartupData()
        {
            try
            {
                await TryToLoadLendingRate();
                await TryToGetNextContractNumber();
            }

            catch (LoadingLendingRatesException laodingLendingRateException)
            {
                MaterialMessageBox.ShowError(
                    $"{laodingLendingRateException.Message}{Environment.NewLine}Błąd: {laodingLendingRateException.InnerException?.Message}",
                    "Błąd");
            }
            catch (GetNextContractNumberException getNextContractNumberException)
            {
                MaterialMessageBox.ShowError(
                    $"{getNextContractNumberException.Message}{Environment.NewLine}Błąd: {getNextContractNumberException.InnerException?.Message}",
                    "Błąd");
            }
        }

        private async Task TryToLoadLendingRate()
        {
            LendingRates = await _contractService.LoadLendingRates();
        }

        private async Task TryToGetNextContractNumber()
        {
            ContractNumber = await _contractService.GetNextContractNumber();
        }

        #endregion
    }
}