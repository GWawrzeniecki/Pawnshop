using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.Interfaces;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Controls.SharedViews.ViewModels
{
    public class SaleInfoViewModel : BindableBase, INavigationAware
    {
        #region PrivateMembers

        private readonly IMapper _mapper;
        private readonly IMessageBoxService _messageBoxService;
        private readonly IContractItemService _contractItemService;
        private IList<UnitMeasure> _contractItemUnitMeasures;
        private UnitMeasure _selectedContractItemUnitMeasure;
        private int? _contractItemQuantity;
        private decimal? _price;
        private string _rack;
        private int? _shelf;
        private IList<Link> _saleLinks;
        private Link _selectedSaleLink;
        private string _saleLinkText;
        private Sale _sale;
        private DateTime _putOnSaleDate;

        #endregion

        #region Constructor

        public SaleInfoViewModel(IContractItemService contractItemService, IMapper mapper, IMessageBoxService messageBoxService)
        {
            _contractItemService = contractItemService;
            _mapper = mapper;
            _messageBoxService = messageBoxService;
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

        public DateTime PutOnSaleDate
        {
            get => _putOnSaleDate;
            set => SetProperty(ref _putOnSaleDate, value);
        }

        #endregion

        #region INavigationAware

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            _sale = navigationContext.Parameters.GetValue<Sale>("sale");
            var isLoaded = await LoadStartupData();
            if (isLoaded)
                MapUnitMeasureFromCurrentDbContext();
            MapSaleToVm();
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

        private async Task<bool> LoadStartupData()
        {
            var success = false;

            try
            {
                await TryToLoadUnitMeasures();
                success = true;
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

            return success;
        }

        private async Task TryToLoadUnitMeasures()
        {
            ContractItemUnitMeasures = await _contractItemService.GetUnitMeasures();
        }

        private void MapUnitMeasureFromCurrentDbContext()
        {
            SelectedContractItemUnitMeasure =
                ContractItemUnitMeasures.FirstOrDefault(c => c.Id == _sale.ContractItem.Category.MeasureId);
        }

        private void MapSaleToVm()
        {
            _mapper.Map(_sale, this);
        }

        #endregion
    }
}