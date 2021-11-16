using AutoMapper;
using BespokeFusion;
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
    public class SaleBasicInfoViewModel : BindableBase, INavigationAware
    {
        #region PrivateMembers

        private string _contractItemName;
        private int _contractItemQuantity;
        private string _contractItemDescription;
        private IList<ContractItemCategory> _contractItemCategories;
        private ContractItemCategory _selectedContractItemCategory;
        private IList<UnitMeasure> _contractItemUnitMeasures;
        private UnitMeasure _selectedContractItemUnitMeasure;
        private decimal _contractItemEstimatedValue;
        private string _contractItemTechnicalCondition;
        private readonly IMapper _mapper;
        private readonly IContractItemService _contractItemService;
        private ContractItem _contractItem;

        #endregion

        #region Constructor

        public SaleBasicInfoViewModel(IMapper mapper, IContractItemService contractItemService)
        {
            _mapper = mapper;
            _contractItemService = contractItemService;
        }

        #endregion

        #region PublicProperties
        public string ContractItemName
        {
            get => _contractItemName;
            set => SetProperty(ref _contractItemName, value);
        }

        public int ContractItemQuantity
        {
            get => _contractItemQuantity;
            set => SetProperty(ref _contractItemQuantity, value);
        }

        public string ContractItemDescription
        {
            get => _contractItemDescription;
            set => SetProperty(ref _contractItemDescription, value);
        }

        public IList<ContractItemCategory> ContractItemCategories
        {
            get => _contractItemCategories;
            set => SetProperty(ref _contractItemCategories, value);
        }

        public IList<UnitMeasure> ContractItemUnitMeasures
        {
            get => _contractItemUnitMeasures;
            set => SetProperty(ref _contractItemUnitMeasures, value);
        }

        public ContractItemCategory SelectedContractItemCategory
        {
            get => _selectedContractItemCategory;
            set => SetProperty(ref _selectedContractItemCategory, value);
        }

        public UnitMeasure SelectedContractItemUnitMeasure
        {
            get => _selectedContractItemUnitMeasure;
            set => SetProperty(ref _selectedContractItemUnitMeasure, value);
        }

        public decimal ContractItemEstimatedValue
        {
            get => _contractItemEstimatedValue;
            set => SetProperty(ref _contractItemEstimatedValue, value);
        }

        public string ContractItemTechnicalCondition
        {
            get => _contractItemTechnicalCondition;
            set => SetProperty(ref _contractItemTechnicalCondition, value);
        }

        #endregion

        #region IDialogAware

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            _contractItem = navigationContext.Parameters.GetValue<ContractItem>("contractItem");
            MapContractItemToVm();
            var success = await LoadStartupData();
            if (!success) return;
            MapCategoryAndMeasureFromCurrentDbContext();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        #endregion IDialogAware

        #region PrivateMethods

        private async Task<bool> LoadStartupData()
        {
            var success = false;

            try
            {
                await TryToLoadContractItemCategories();
                await TryToLoadUnitMeasures();
                success = true;
            }
            catch (LoadingContractItemCategoriesException loadingContractItemCategoriesException)
            {
                MaterialMessageBox.ShowError(
                    $"{loadingContractItemCategoriesException.Message}{Environment.NewLine}Błąd: {loadingContractItemCategoriesException.InnerException?.Message}",
                    "Błąd");
            }
            catch (LoadingUnitMeasuresException loadingUnitMeasuresException)
            {
                MaterialMessageBox.ShowError(
                    $"{loadingUnitMeasuresException.Message}{Environment.NewLine}Błąd: {loadingUnitMeasuresException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }

            return success;
        }

        private async Task TryToLoadContractItemCategories()
        {
            ContractItemCategories = await _contractItemService.GetContractItemCategories();
        }

        private async Task TryToLoadUnitMeasures()
        {
            ContractItemUnitMeasures = await _contractItemService.GetUnitMeasures();
        }

        private void MapContractItemToVm()
        {
            _mapper.Map(_contractItem, this);
        }

        private void MapCategoryAndMeasureFromCurrentDbContext()
        {
            SelectedContractItemCategory = ContractItemCategories
                .First(ctc => ctc.Id == SelectedContractItemCategory.Id);

            SelectedContractItemUnitMeasure = ContractItemUnitMeasures
                .First(cim => cim.Id == SelectedContractItemUnitMeasure.Id);
        }

        #endregion

    }
}
