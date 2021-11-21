﻿using BespokeFusion;
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
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PawnShop.Modules.Commodity.Dialogs.ViewModels
{
    public class PreviewPutOnSaleDialogViewModel : BindableBase, IDialogAware, IRegionManagerAware
    {

        #region PrivateMembers

        private readonly IEventAggregator _eventAggregator;
        private readonly IContainerProvider _containerProvider;
        private string _title;
        private DialogMode _dialogMode;
        private ContractItem _contractItem;
        private string _secondGroupBoxHeaderName;
        private Visibility _putOnSaleButtonVisibility;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _putOnSaleCommand;

        #endregion

        #region Constructor

        public PreviewPutOnSaleDialogViewModel(IEventAggregator eventAggregator, IContainerProvider containerProvider)
        {
            _eventAggregator = eventAggregator;
            _containerProvider = containerProvider;
        }

        #endregion

        #region PublicProperties

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string SecondGroupBoxHeaderName
        {
            get => _secondGroupBoxHeaderName;
            set => SetProperty(ref _secondGroupBoxHeaderName, value);
        }

        public DialogMode DialogMode
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
            DialogMode = parameters.GetValue<DialogMode>("dialogMode");
            _contractItem = parameters.GetValue<ContractItem>("contractItem");
            NavigateToBasicInfo();
            OnDialogMode(DialogMode);
        }

        public event Action<IDialogResult> RequestClose;

        #endregion IDialogAware

        #region PrivateMethods
        private void NavigateToBasicInfo()
        {
            RegionManager.RequestNavigate(RegionNames.PreviewPutOnSaleDialogBasicInfoRegion, nameof(Controls.SharedViews.Views.SaleBaseInfo), new NavigationParameters { { "contractItem", _contractItem } });
        }

        private void OnDialogMode(DialogMode dialogMode)
        {
            switch (dialogMode)
            {
                case DialogMode.ReadOnly:
                    NavigateToAdditionalInfo();
                    SetSecondGroupBoxHeaderName("Informacje dodatkowe");
                    SetPutOnSaleButtonVisibility(Visibility.Hidden);
                    break;
                case DialogMode.Editable:
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
                    RegionManager.RequestNavigate(RegionNames.PreviewPutOnSaleDialogContentRegion, Constants.Laptop, new NavigationParameters { { "dialogMode", _dialogMode }, { "laptop", _contractItem.Laptop } });
                    break;
                default:
                    throw new NotImplementedException(_contractItem.Category.Category);
            }
        }

        private void NavigateToSale()
        {
            RegionManager.RequestNavigate(RegionNames.PreviewPutOnSaleDialogContentRegion, nameof(PutOnSale), new NavigationParameters { { "putOnSaleCommand", PutOnSaleCommand }, });
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