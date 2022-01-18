using PawnShop.Business.Models;
using PawnShop.Core.Dialogs;
using PawnShop.Core.Enums;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.Core.ViewModel;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Commodity.Events;
using PawnShop.Modules.Commodity.RegionContext;
using PawnShop.Services.Interfaces;
using Prism;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawnShop.Modules.Commodity.Base
{
    public abstract class GoodsBaseViewModel : BindableBase, ITabItemViewModel, IActiveAware
    {
        #region PrivateMembers

        private readonly IDialogService _dialogService;
        private readonly IMessageBoxService _messageBoxService;
        private IList<ContractItem> _contractItems;
        private ContractItem _selectedContractItem;
        private bool _isBusy;
        private CommodityTabRegionContext _commodityTabRegionContext;
        private bool _isActive;

        #endregion

        #region Constructor

        protected GoodsBaseViewModel(IEventAggregator eventAggregator, IDialogService dialogService, string headerName, IMessageBoxService messageBoxService)
        {
            _dialogService = dialogService;
            _messageBoxService = messageBoxService;
            Header = headerName;
            ContractItems = new List<ContractItem>();
            eventAggregator.GetEvent<RefreshDataGridEvent>().Subscribe(RefreshDataGrid);
            eventAggregator.GetEvent<TaskBarButtonClickEvent>().Subscribe(TaskBarButtonClick);
        }

        #endregion

        #region PublicProperties

        public IList<ContractItem> ContractItems
        {
            get => _contractItems;
            set => SetProperty(ref _contractItems, value);
        }

        public ContractItem SelectedContractItem
        {
            get => _selectedContractItem;
            set
            {
                SetProperty(ref _selectedContractItem, value);
                CommodityTabRegionContext.CanExecute = value is not null && IsActive;
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public CommodityTabRegionContext CommodityTabRegionContext
        {
            get => _commodityTabRegionContext;
            set => SetProperty(ref _commodityTabRegionContext, value);
        }

        #endregion

        #region ITabItemViewModel

        public string Header { get; set; }

        #endregion

        #region RefreshDataGridEvent

        private async void RefreshDataGrid(ContractItemQueryData obj)
        {
            if (!IsActive)
                return;

            try
            {
                IsBusy = true;
                ContractItems = await TryToRefreshDataGrid(obj);
            }
            catch (LoadingContractItemsException loadingContractItemsException)
            {
                _messageBoxService.ShowError(
                    $"{loadingContractItemsException.Message}{Environment.NewLine}Błąd: {loadingContractItemsException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region TaskBarButtonClickEvent

        private void TaskBarButtonClick(DialogMode dialogMode)
        {
            if (!IsActive || SelectedContractItem is null)
                return;

            _dialogService.ShowPreviewPutOnSaleDialog(null, "Podgląd towaru", dialogMode, SelectedContractItem);
        }

        #endregion

        #region ProtectedMethods

        protected async void LoadContractItems()
        {
            try
            {
                IsBusy = true;
                ContractItems = await TryToLoadContractItems();
            }
            catch (LoadingContractItemsException loadingContractItemsException)
            {
                _messageBoxService.ShowError(
                    $"{loadingContractItemsException.Message}{Environment.NewLine}Błąd: {loadingContractItemsException.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected abstract Task<IList<ContractItem>> TryToRefreshDataGrid(ContractItemQueryData contractItemQueryData);


        protected abstract Task<IList<ContractItem>> TryToLoadContractItems();

        #endregion

        #region PrivateMethods

        #endregion

        #region IActiveAware

#pragma warning disable CS0067 // The event 'GoodsBaseViewModel.IsActiveChanged' is never used
        public event EventHandler IsActiveChanged;
#pragma warning restore CS0067 // The event 'GoodsBaseViewModel.IsActiveChanged' is never used
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                CommodityTabRegionContext.CanExecute = SelectedContractItem is not null && IsActive;
            }
        }

        #endregion
    }
}