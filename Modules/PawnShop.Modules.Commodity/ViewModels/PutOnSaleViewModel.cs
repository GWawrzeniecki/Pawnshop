using PawnShop.Business.Models;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Commodity.Validators;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Commodity.ViewModels
{
    public class PutOnSaleViewModel : ViewModelBase<PutOnSaleViewModel>, INavigationAware
    {
        #region PrivateMembers

        private readonly IContractItemService _contractItemService;
        private readonly IMessageBoxService _messageBoxService;
        private IList<UnitMeasure> _contractItemUnitMeasures;
        private UnitMeasure _selectedContractItemUnitMeasure;
        private int? _contractItemQuantity;
        private decimal? _price;
        private string _rack;
        private int? _shelf;
        private IList<Link> _saleLinks;
        private Link _selectedSaleLink;
        private string _saleLinkText;
        private DelegateCommand _addLinkCommand;
        private DelegateCommand _putOnSaleCommand;
        private int? _contractItemQuantityForSale;

        #endregion

        #region Constructor

        public PutOnSaleViewModel(IContractItemService contractItemService, PutOnSaleValidator putOnSaleValidator, IMessageBoxService messageBoxService) : base(putOnSaleValidator)
        {
            _contractItemService = contractItemService;
            _messageBoxService = messageBoxService;
            SaleLinks = new ObservableCollection<Link>();
        }

        #endregion

        #region PublicProperties

        public IList<UnitMeasure> ContractItemUnitMeasures
        {
            get => _contractItemUnitMeasures;
            set => SetProperty(ref _contractItemUnitMeasures, value);
        }

        public UnitMeasure SelectedContractItemUnitMeasure
        {
            get => _selectedContractItemUnitMeasure;
            set => SetProperty(ref _selectedContractItemUnitMeasure, value);
        }

        public int? ContractItemQuantity
        {
            get => _contractItemQuantity;
            set => SetProperty(ref _contractItemQuantity, value);
        }

        public int? ContractItemQuantityForSale
        {
            get => _contractItemQuantityForSale;
            set => SetProperty(ref _contractItemQuantityForSale, value);
        }

        public decimal? Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public string Rack
        {
            get => _rack;
            set => SetProperty(ref _rack, value);
        }

        public int? Shelf
        {
            get => _shelf;
            set => SetProperty(ref _shelf, value);
        }

        public IList<Link> SaleLinks
        {
            get => _saleLinks;
            set => SetProperty(ref _saleLinks, value);
        }

        public Link SelectedSaleLink
        {
            get => _selectedSaleLink;
            set => SetProperty(ref _selectedSaleLink, value);
        }

        public string SaleLinkText
        {
            get => _saleLinkText;
            set => SetProperty(ref _saleLinkText, value);
        }

        public bool CanExecutePutOnSale => !HasErrors;

        public ContractItem ContractItem { get; private set; }

        #endregion

        #region Commands

        public DelegateCommand AddLinkCommand => _addLinkCommand ??= new DelegateCommand(AddLink, CanExecuteAddLink)
            .ObservesProperty(() => SaleLinkText);

        #endregion

        #region CommandMethods

        private void AddLink()
        {
            SaleLinks.Add(new Link { Link1 = SaleLinkText });
            SaleLinkText = string.Empty;
        }

        private bool CanExecuteAddLink()
        {
            return !string.IsNullOrEmpty(SaleLinkText);
        }

        #endregion

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ContractItem = navigationContext.Parameters.GetValue<ContractItem>("contractItem");
            ContractItemQuantityForSale = ContractItem.Amount - ContractItem.Sales.Sum(s => s.Quantity);
            _putOnSaleCommand = navigationContext.Parameters.GetValue<DelegateCommand>("putOnSaleCommand");
            _putOnSaleCommand.ObservesCanExecute(() => CanExecutePutOnSale);
            Commands.Add(_putOnSaleCommand);
            _putOnSaleCommand.RaiseCanExecuteChanged();
            LoadStartupData();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        #endregion

        #region PrivateMethods

        private async void LoadStartupData()
        {
            try
            {
                await TryToLoadUnitMeasures();
            }
            catch (LoadingUnitMeasuresException loadingUnitMeasuresException)
            {
                _messageBoxService.ShowError(
                    $"{loadingUnitMeasuresException.Message}{Environment.NewLine}Błąd: {loadingUnitMeasuresException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }
        private async Task TryToLoadUnitMeasures()
        {
            ContractItemUnitMeasures = await _contractItemService.GetUnitMeasures();
        }

        #endregion

        #region viewModelBase

        protected override PutOnSaleViewModel GetInstance()
        {
            return this;
        }

        #endregion viewModelBase
    }
}
