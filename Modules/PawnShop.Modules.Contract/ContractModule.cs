using PawnShop.Core.Regions;
using PawnShop.Modules.Contract.Dialogs.ViewModels;
using PawnShop.Modules.Contract.Dialogs.Views;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Contract.Validators;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Modules.Contract.Views;
using PawnShop.Services.Implementations;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PawnShop.Modules.Contract
{
    public class ContractModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ContractModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.MenuRegion].Add(containerProvider.Resolve<ContractHamburgerMenuItem>());
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.Contract, ContractViewModel>();
            containerRegistry.RegisterForNavigation<ClientData, ClientDataViewModel>();
            containerRegistry.RegisterForNavigation<ContractData, ContractDataViewModel>();
            containerRegistry.RegisterForNavigation<Summary, SummaryViewModel>();
            containerRegistry.RegisterForNavigation<RenewContractData, RenewContractDataViewModel>();
            containerRegistry.RegisterForNavigation<RenewContractPayment, RenewContractPaymentViewModel>();
            containerRegistry.RegisterForNavigation<BuyBackContractData, BuyBackContractDataViewModel>();
            containerRegistry.RegisterForNavigation<BuyBackContractItems, BuyBackContractItemsViewModel>();
            containerRegistry.RegisterForNavigation<BuyBackContractPayment, BuyBackContractPaymentViewModel>();
            containerRegistry.RegisterSingleton<ClientDataHamburgerMenuItem>();
            containerRegistry.RegisterSingleton<ContractDataHamburgerMenuItem>();
            containerRegistry.RegisterSingleton<SummaryHamburgerMenuItem>();
            containerRegistry.RegisterSingleton<RenewContractDataHamburgerMenuItem>();
            containerRegistry.RegisterSingleton<RenewContractPaymentHamburgerMenuItem>();
            containerRegistry.RegisterSingleton<BuyBackContractDataHamburgerMenuItem>();
            containerRegistry.RegisterSingleton<BuyBackContractItemsHamburgerMenuItem>();
            containerRegistry.RegisterSingleton<BuyBackContactPaymentHamburgerMenuItem>();
            containerRegistry.RegisterSingleton<ContractValidator>();
            containerRegistry.RegisterSingleton<CreateContractValidator>();
            containerRegistry.RegisterSingleton<AddContractItemValidator>();
            containerRegistry.Register<IContractService, ContractService>();
            containerRegistry.RegisterSingleton<ICalculateService, CalculateService>();
            containerRegistry.RegisterSingleton<IConfigurationService, ConfigurationService>();
            containerRegistry.RegisterSingleton<IPdfService, PdfService>();
            containerRegistry.RegisterDialog<AddContractItemDialog, AddContractItemDialogViewModel>();
        }
    }
}