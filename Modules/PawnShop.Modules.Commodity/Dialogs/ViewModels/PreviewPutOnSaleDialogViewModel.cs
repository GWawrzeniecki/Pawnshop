﻿using AutoMapper;
using BespokeFusion;
using PawnShop.Business.Models;
using PawnShop.Core.Constants;
using PawnShop.Core.Enums;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Commodity.Events;
using PawnShop.Modules.Commodity.ViewModels;
using PawnShop.Modules.Commodity.Views;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PawnShop.Modules.Commodity.Dialogs.ViewModels
{
    public class PreviewPutOnSaleDialogViewModel : BindableBase, IDialogAware, IRegionManagerAware
    {

        #region PrivateMembers

        private readonly IMapper _mapper;
        private readonly IContractItemService _contractItemService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IContainerProvider _containerProvider;
        private string _title;
        private PreviewPutOnSaleDialogMode _dialogMode;
        private ContractItem _contractItem;
        private string _contractItemName;
        private int _contractItemQuantity;
        private string _contractItemDescription;
        private IList<ContractItemCategory> _contractItemCategories;
        private ContractItemCategory _selectedContractItemCategory;
        private IList<UnitMeasure> _contractItemUnitMeasures;
        private UnitMeasure _selectedContractItemUnitMeasure;
        private decimal _contractItemEstimatedValue;
        private string _contractItemTechnicalCondition;
        private string _secondGroupBoxHeaderName;
        private Visibility _putOnSaleButtonVisibility;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _putOnSaleCommand;

        #endregion

        #region Constructor

        public PreviewPutOnSaleDialogViewModel(IMapper mapper, IContractItemService contractItemService, IEventAggregator eventAggregator, IContainerProvider containerProvider)
        {
            _mapper = mapper;
            _contractItemService = contractItemService;
            _eventAggregator = eventAggregator;
            _containerProvider = containerProvider;
            ContractItemUnitMeasures = new List<UnitMeasure>();
            LoadStartupData();
        }

        #endregion

        #region PublicProperties

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

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

        public string SecondGroupBoxHeaderName
        {
            get => _secondGroupBoxHeaderName;
            set => SetProperty(ref _secondGroupBoxHeaderName, value);
        }

        public PreviewPutOnSaleDialogMode DialogMode
        {
            get => _dialogMode;
            set => SetProperty(ref _dialogMode, value);
        }

        public IRegionManager RegionManager
        {
            get;
            set;
        }

        public Visibility PutOnSaleButtonVisibility
        {
            get => _putOnSaleButtonVisibility;
            set => SetProperty(ref _putOnSaleButtonVisibility, value);
        }

        #endregion

        #region Commands

        public DelegateCommand PutOnSaleCommand =>
            _putOnSaleCommand ??= new DelegateCommand(PutOnSale);


        public DelegateCommand CancelCommand =>
            _cancelCommand ??= new DelegateCommand(Cancel);

        #endregion

        #region CommandsMethods

        private async void PutOnSale()
        {
            try
            {
                await TryToPutOnSale();
                MaterialMessageBox.Show("Pomyślnie wystawiono towar na sprzedaż.");
                _eventAggregator.GetEvent<RefreshDataGridEvent>().Publish(new ContractItemQueryData());
                CloseDialog(new DialogResult(ButtonResult.OK));
            }
            catch (InsertSaleException insertSaleException)
            {
                MaterialMessageBox.ShowError(
                    $"{insertSaleException.Message}{Environment.NewLine}Błąd: {insertSaleException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private void Cancel()
        {
            RequestClose.Invoke(new DialogResult(ButtonResult.Cancel));
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
            Title = parameters.GetValue<string>("title");
            DialogMode = parameters.GetValue<PreviewPutOnSaleDialogMode>("dialogMode");
            _contractItem = parameters.GetValue<ContractItem>("contractItem");
            OnDialogMode(DialogMode);
        }

        public event Action<IDialogResult> RequestClose;

        #endregion IDialogAware

        #region PrivateMethods

        private async void LoadStartupData()
        {
            try
            {
                await TryToLoadContractItemCategories();
                await TryToLoadUnitMeasures();
                MapContractItemToVm();
                MapCategoryAndMeasureFromCurrentDbContext();
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
        }


        private async Task TryToLoadContractItemCategories()
        {
            ContractItemCategories = await _contractItemService.GetContractItemCategories();
        }

        private async Task TryToLoadUnitMeasures()
        {
            var measures = await _contractItemService.GetUnitMeasures();
            foreach (var unitMeasure in measures)
            {
                ContractItemUnitMeasures.Add(unitMeasure);
            }
        }

        private void MapCategoryAndMeasureFromCurrentDbContext()
        {
            SelectedContractItemCategory = ContractItemCategories
                .First(ctc => ctc.Id == SelectedContractItemCategory.Id);

            SelectedContractItemUnitMeasure = ContractItemUnitMeasures
                .First(cim => cim.Id == SelectedContractItemUnitMeasure.Id);
        }

        private void OnDialogMode(PreviewPutOnSaleDialogMode dialogMode)
        {
            switch (dialogMode)
            {
                case PreviewPutOnSaleDialogMode.Preview:
                    NavigateToAdditionalInfo();
                    SetSecondGroupBoxHeaderName("Informacje dodatkowe");
                    SetPutOnSaleButtonVisibility(Visibility.Hidden);
                    break;
                case PreviewPutOnSaleDialogMode.Sale:
                    NavigateToSale();
                    SetSecondGroupBoxHeaderName("Sprzedaż");
                    SetPutOnSaleButtonVisibility(Visibility.Visible);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dialogMode), dialogMode, null);
            }
        }

        private void NavigateToAdditionalInfo()
        {
            switch (_contractItem.Category.Category)
            {
                case Constants.Laptop:
                    RegionManager.RequestNavigate(RegionNames.PreviewPutOnSaleDialogContentRegion, Constants.Laptop, new NavigationParameters { { "dialogMode", _dialogMode }, { "laptop", _contractItem.Laptop } }); // tutaj
                    break;
                default:
                    throw new NotImplementedException(_contractItem.Category.Category);
            }
        }

        private void NavigateToSale()
        {
            RegionManager.RequestNavigate(RegionNames.PreviewPutOnSaleDialogContentRegion, nameof(PutOnSale), new NavigationParameters { { "measures", ContractItemUnitMeasures }, { "putOnSaleCommand", PutOnSaleCommand }, });
        }

        private void MapContractItemToVm()
        {
            _mapper.Map(_contractItem, this);
        }

        private void SetSecondGroupBoxHeaderName(string name)
        {
            SecondGroupBoxHeaderName = name;
        }
        private async Task TryToPutOnSale()
        {
            var putOnSale = RegionManager.Regions[RegionNames.PreviewPutOnSaleDialogContentRegion].ActiveViews
                .OfType<PutOnSale>().First();
            var putOnSaleViewModel = putOnSale.DataContext as PutOnSaleViewModel;
            var sale = CreateAndMapSellEntity(putOnSaleViewModel);
            await TryToInsertSale(sale);
        }

        private async Task TryToInsertSale(Sale sale)
        {
            try
            {
                using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
                await unitOfWork.SaleRepository.InsertAsync(sale);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new InsertSaleException("Wystapil blad podczas wystawiania towaaru na sprzedaz", e);
            }
        }

        private void SetPutOnSaleButtonVisibility(Visibility visibility) => PutOnSaleButtonVisibility = visibility;

        private Sale CreateAndMapSellEntity(PutOnSaleViewModel putOnSaleViewModel)
        {
            return new Sale()
            {
                ContractItemId = _contractItem.ContractItemId,
                SalePrice = putOnSaleViewModel.Price.GetValueOrDefault(),
                PutOnSaleDate = DateTime.Now,
                LocalSale = new LocalSale() { Rack = putOnSaleViewModel.Rack, Shelf = putOnSaleViewModel.Shelf.GetValueOrDefault() },
                Links = putOnSaleViewModel.SaleLinks
            };
        }

        private void CloseDialog(IDialogResult dialogResult)
        {
            RequestClose.Invoke(dialogResult);
        }

        #endregion
    }
}