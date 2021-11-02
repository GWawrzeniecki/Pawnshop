﻿using PawnShop.Business.Models;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.Modules.Commodity.Base;
using PawnShop.Services.DataService;
using Prism.Events;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawnShop.Modules.Commodity.ViewModels
{
    public class GoodsForSaleGridViewModel : GoodsBaseViewModel
    {
        #region PrivateMembers

        private readonly IContainerProvider _containerProvider;

        #endregion

        #region Constructor

        public GoodsForSaleGridViewModel(IContainerProvider containerProvider, IEventAggregator eventAggregator, IDialogService dialogService) : base(eventAggregator, dialogService, "Do wystawienia na sprzedaż")
        {
            _containerProvider = containerProvider;
            LoadContractItems();
        }

        #endregion

        #region PublicProperties

        #endregion

        #region GoodsBaseViewModel

        protected override async Task<IList<ContractItem>> TryToLoadContractItems()
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return await unitOfWork.ContractItemRepository.GetTopContractItemsForSale(100);
        }

        protected override async Task<IList<ContractItem>> TryToRefreshDataGrid(ContractItemQueryData contractItemQueryData)
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            return await unitOfWork.ContractItemRepository.GetTopContractItemsForSale(contractItemQueryData, 100);
        }

        #endregion

        #region PrivateMethods

        #endregion
    }
}