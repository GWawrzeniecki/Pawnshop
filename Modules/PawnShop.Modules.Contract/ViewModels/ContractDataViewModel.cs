using BespokeFusion;
using PawnShop.Business.Models;
using PawnShop.Core.Dialogs;
using PawnShop.Core.HamburgerMenu.Implementations;
using PawnShop.Core.HamburgerMenu.Interfaces;
using PawnShop.Core.ScopedRegion;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Contract.Services;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class ContractDataViewModel : BindableBase, IRegionManagerAware, INavigationAware, IHamburgerMenuEnabled
    {
        #region Private members

        private IList<LendingRate> _lendingRates;
        private readonly IContractService _contractService;
        private readonly IDialogService _dialogService;
        private readonly ICalculateService _calculateService;
        private string _contractNumber;
        private LendingRate _selectedLendingRate;
        private DateTime? _rePurchaseDateTime;
        private DelegateCommand _addContractItemCommand;
        private IList<ContractItem> _boughtContractItems;
        private Client _dealMaker;
        #endregion

        #region constructor

        public ContractDataViewModel(IContractService contractService,
            IDialogService dialogService, ICalculateService calculateService, IContainerProvider containerProvider)
        {
            _contractService = contractService;
            _dialogService = dialogService;
            _calculateService = calculateService;
            HamburgerMenuItem = containerProvider.Resolve<SummaryHamburgerMenuItem>();
            BoughtContractItems = new List<ContractItem>();
            LoadStartupData();


        }

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


        public LendingRate SelectedLendingRate
        {
            get => _selectedLendingRate;
            set
            {
                SetProperty(ref _selectedLendingRate, value);
                RepurchaseDate = value == null ? default : DateTime.Today.AddDays(value.Days);
                RaisePropertyChanged(nameof(IsNextButtonEnabled));
                RaisePropertyChanged(nameof(RePurchasePrice));
                AddContractItemCommand.RaiseCanExecuteChanged();
                (this as IHamburgerMenuEnabled).IsEnabled = IsNextButtonEnabled;

            }
        }


        public DateTime? RepurchaseDate
        {
            get => _rePurchaseDateTime;
            set => SetProperty(ref _rePurchaseDateTime, value);
        }





        public decimal RePurchasePrice
        {
            get
            {
                if (SelectedLendingRate == null)
                    return 0;
                return _calculateService.CalculateContractAmount(BoughtContractItems.Sum(item => item.EstimatedValue),
                    SelectedLendingRate);


            }
        }



        public IList<ContractItem> BoughtContractItems
        {
            get => _boughtContractItems;
            set
            {
                SetProperty(ref _boughtContractItems, value);
                RaisePropertyChanged(nameof(RePurchasePrice));
                RaisePropertyChanged(nameof(IsNextButtonEnabled));
                (this as IHamburgerMenuEnabled).IsEnabled = IsNextButtonEnabled;
            }
        }


        public bool IsNextButtonEnabled => SelectedLendingRate != null && BoughtContractItems.Count > 0;

        #endregion


        #region Commands



        public DelegateCommand AddContractItemCommand =>
            _addContractItemCommand ??= new DelegateCommand(AddContractItem, CanExecuteAddContractItem);



        #endregion Commands



        #region CommandMethods



        private bool CanExecuteAddContractItem()
        {
            return SelectedLendingRate != null;
        }

        private void AddContractItem()
        {
            _dialogService.ShowAddContractItemDialog(r =>
            {
                if (r.Result != ButtonResult.OK) return;
                BoughtContractItems.Add(r.Parameters.GetValue<ContractItem>("contractItem"));
                BoughtContractItems = new List<ContractItem>(BoughtContractItems);



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

            catch (LoadingLendingRatesException loadingLendingRatesException)
            {
                MaterialMessageBox.ShowError(
                    $"{loadingLendingRatesException.Message}{Environment.NewLine}Błąd: {loadingLendingRatesException.InnerException?.Message}",
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

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _dealMaker = navigationContext.Parameters.GetValue<Client>("DealMaker");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add("ContractItems", BoughtContractItems);
            navigationContext.Parameters.Add("LendingRate", SelectedLendingRate);
            navigationContext.Parameters.Add("StartDate", DateTime.Now);
            navigationContext.Parameters.Add("ContractNumber", ContractNumber);
            navigationContext.Parameters.Add("DealMaker", _dealMaker);
        }

        #endregion

        #region IHamburgerMenuEnabled

        public HamburgerMenuItemBase HamburgerMenuItem { get; set; }

        #endregion

    }
}