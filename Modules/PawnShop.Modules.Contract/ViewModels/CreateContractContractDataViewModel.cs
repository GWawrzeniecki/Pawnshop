using PawnShop.Business.Models;
using PawnShop.Core.Dialogs;
using PawnShop.Core.HamburgerMenu.Implementations;
using PawnShop.Core.HamburgerMenu.Interfaces;
using PawnShop.Core.ScopedRegion;
using PawnShop.Core.Tasks;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.MenuItem;
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
    public class CreateContractContractDataViewModel : BindableBase, IRegionManagerAware, INavigationAware, IHamburgerMenuEnabled
    {
        #region Private members

        private NotifyTask<IList<LendingRate>> _lendingRates;
        private readonly IContractService _contractService;
        private readonly IDialogService _dialogService;
        private readonly ICalculateService _calculateService;
        private readonly IMessageBoxService _messageBoxService;
        private NotifyTask<string> _contractNumber;
        private LendingRate _selectedLendingRate;
        private DateTime? _rePurchaseDateTime;
        private DelegateCommand _addContractItemCommand;
        private IList<ContractItem> _boughtContractItems;
        private Client _dealMaker;
        private Func<Task> _callBack;
        #endregion

        #region constructor

        public CreateContractContractDataViewModel(IContractService contractService,
            IDialogService dialogService, ICalculateService calculateService, IContainerProvider containerProvider, IMessageBoxService messageBoxService)
        {
            _contractService = contractService;
            _dialogService = dialogService;
            _calculateService = calculateService;
            _messageBoxService = messageBoxService;
            HamburgerMenuItem = containerProvider.Resolve<CreateContractSummaryHamburgerMenuItem>();
            BoughtContractItems = new List<ContractItem>();
            LendingRates = NotifyTask.Create(LoadLendingRates);
            ContractNumber = NotifyTask.Create(GetNextContractNumber);
        }

        #endregion

        #region IRegionManagerAware

        public IRegionManager RegionManager { get; set; }

        #endregion

        #region PublicProperties

        public NotifyTask<IList<LendingRate>> LendingRates
        {
            get => _lendingRates;
            set => SetProperty(ref _lendingRates, value);
        }

        public NotifyTask<string> ContractNumber
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

        private async Task<IList<LendingRate>> LoadLendingRates()
        {
            var lendingRates = Enumerable.Empty<LendingRate>();
            try
            {
                lendingRates = await _contractService.LoadLendingRates();
            }
            catch (LoadingLendingRatesException loadingLendingRatesException)
            {
                _messageBoxService.ShowError(
                    $"{loadingLendingRatesException.Message}{Environment.NewLine}Błąd: {loadingLendingRatesException.InnerException?.Message}",
                    "Błąd");

            }

            return lendingRates.ToList();
        }
        private async Task<string> GetNextContractNumber()
        {
            var contractNumber = string.Empty;

            try
            {
                contractNumber = await _contractService.GetNextContractNumber();
            }
            catch (GetNextContractNumberException getNextContractNumberException)
            {
                _messageBoxService.ShowError(
                    $"{getNextContractNumberException.Message}{Environment.NewLine}Błąd: {getNextContractNumberException.InnerException?.Message}",
                    "Błąd");
            }

            return contractNumber;
        }

        #endregion

        #region INavigationAware
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _dealMaker = navigationContext.Parameters.GetValue<Client>("DealMaker") ?? _dealMaker;
            _callBack = navigationContext.Parameters.GetValue<Func<Task>>("CallBack");

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
            navigationContext.Parameters.Add("ContractNumber", ContractNumber.Result);
            navigationContext.Parameters.Add("DealMaker", _dealMaker);
            navigationContext.Parameters.Add("CallBack", _callBack);
        }

        #endregion

        #region IHamburgerMenuEnabled

        public HamburgerMenuItemBase HamburgerMenuItem { get; set; }

        #endregion

    }
}