using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PawnShop.Controls.Validators;
using PawnShop.Core;
using PawnShop.Core.SharedVariables;
using PawnShop.DataAccess.Data;
using PawnShop.Mapper.Profiles;
using PawnShop.Modules.Contract.Dialogs.ViewModels;
using PawnShop.Modules.Contract.Dialogs.Views;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Contract.Validators;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Modules.Contract.Views;
using PawnShop.Modules.Login.Validators;
using PawnShop.Services.DataService;
using PawnShop.Services.Implementations;
using PawnShop.Services.Interfaces;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;
using Prism.Unity;
using System.Configuration;

namespace IntegrationTests.Base
{
    public class IntegrationTestBase<T> where T : class
    {
        protected T ViewModel;
        protected UnityContainerExtension ContainerProvider;
        private DbContextOptionsBuilder<PawnshopContext> _dbContextOptionsBuilder;
        protected PawnshopContext PawnshopContext => new(_dbContextOptionsBuilder.Options);
        protected IntegrationTestBase()
        {
            Setup();
        }

        private void Setup()
        {
            ContainerProvider = new UnityContainerExtension();
            ContainerProvider.Register<IDialogService, DialogService>();
            ContainerProvider.Register<IModuleCatalog, ModuleCatalog>();
            ContainerProvider.Register<IModuleManager, ModuleManager>();
            ContainerProvider.Register<IModuleInitializer, ModuleInitializer>();
            ContainerProvider.RegisterSingleton<IEventAggregator, EventAggregator>();
            ContainerProvider.RegisterSingleton<IRegionManager, RegionManager>();
            ContainerProvider.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
            ContainerProvider.Register<IUIService, UIService>();
            ContainerProvider.Register<IShellService, ShellService>();
            ContainerProvider.RegisterSingleton<ISessionContext, SessionContext>();
            ContainerProvider.RegisterSingleton<IConfigData, ConfigData>();
            ContainerProvider.RegisterSingleton<IUserSettings, UserSettings>();
            ContainerProvider.Register<IValidatorService, ValidatorService>();
            ContainerProvider.Register<IClientService, ClientService>();
            ContainerProvider.Register<IContractItemService, ContractItemService>();
            ContainerProvider.Register<IConfigurationService, ConfigurationService>();
            ContainerProvider.Register<IPrintService, PrintService>();
            ContainerProvider.Register<IApiService, ApiService>();
            ContainerProvider.Register<AddClientValidator>();
            ContainerProvider.RegisterSingleton<ISecretManagerService, SecretManagerService>();
            ContainerProvider.RegisterSingleton<IHashService, HashService>();
            ContainerProvider.RegisterSingleton<IAesService, AesService>();
            ContainerProvider.Register<ILoginService, LoginService>();
            ContainerProvider.Register<LoginDialogValidator>();
            ContainerProvider.Register<ICalculateService, CalculateService>();
            ContainerProvider.Register<IMessageBoxService, FakeMessageBoxService>();
            ContainerProvider.Register<IContractService, ContractService>();
            ContainerProvider.Register<IPdfService, PdfService>();
            ContainerProvider.RegisterForNavigation<Contract, ContractViewModel>();
            ContainerProvider.RegisterForNavigation<CreateContractClientData, CreateContractClientDataViewModel>();
            ContainerProvider.RegisterForNavigation<CreateContractContractData, CreateContractContractDataViewModel>();
            ContainerProvider.Register<CreateContractSummaryViewModel>();
            ContainerProvider.RegisterForNavigation<CreateContractSummary, CreateContractSummaryViewModel>();
            ContainerProvider.RegisterForNavigation<RenewContractData, RenewContractDataViewModel>();
            ContainerProvider.RegisterForNavigation<RenewContractPayment, RenewContractPaymentViewModel>();
            ContainerProvider.RegisterForNavigation<BuyBackContractData, BuyBackContractDataViewModel>();
            ContainerProvider.RegisterForNavigation<BuyBackContractItems, BuyBackContractItemsViewModel>();
            ContainerProvider.RegisterForNavigation<BuyBackContractPayment, BuyBackContractPaymentViewModel>();
            ContainerProvider.RegisterSingleton<CreateContractClientDataHamburgerMenuItem>();
            ContainerProvider.RegisterSingleton<CreateContractContractDataHamburgerMenuItem>();
            ContainerProvider.RegisterSingleton<CreateContractSummaryHamburgerMenuItem>();
            ContainerProvider.RegisterSingleton<RenewContractDataHamburgerMenuItem>();
            ContainerProvider.RegisterSingleton<RenewContractPaymentHamburgerMenuItem>();
            ContainerProvider.RegisterSingleton<BuyBackContractDataHamburgerMenuItem>();
            ContainerProvider.RegisterSingleton<BuyBackContractItemsHamburgerMenuItem>();
            ContainerProvider.RegisterSingleton<BuyBackContactPaymentHamburgerMenuItem>();
            ContainerProvider.RegisterSingleton<ContractValidator>();
            ContainerProvider.RegisterSingleton<CreateContractValidator>();
            ContainerProvider.RegisterSingleton<AddContractItemValidator>();
            ContainerProvider.RegisterSingleton<ContractHamburgerMenuItem>();
            ContainerProvider.Register<IContractService, ContractService>();
            ContainerProvider.RegisterSingleton<ICalculateService, CalculateService>();
            ContainerProvider.RegisterSingleton<IConfigurationService, ConfigurationService>();
            ContainerProvider.RegisterSingleton<IPdfService, PdfService>();
            ContainerProvider.Register<IEnvironmentVariableService, EnvironmentVariableService>();
            ContainerProvider.RegisterDialog<AddContractItemDialog, AddContractItemDialogViewModel>();


            var dbContextOptionsBuilder = new DbContextOptionsBuilder<PawnshopContext>();
            dbContextOptionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["PawnShopDatabaseTests"].ConnectionString);
            _dbContextOptionsBuilder = dbContextOptionsBuilder;
            ContainerProvider.Register<IUnitOfWork>(() => new UnitOfWork(ContainerProvider, dbContextOptionsBuilder.Options));
            ConfigureMapper(ContainerProvider);
            ViewModel = ContainerProvider.Resolve<T>();
        }

        private void ConfigureMapper(IContainerRegistry containerRegistry)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AddClientViewModelToClientProfile>();
                cfg.AddProfile<AddContractItemDialogViewModelToContractItemProfile>();
                cfg.AddProfile<LaptopViewModelToLaptopProfile>();
                cfg.AddProfile<InsertContractToContractProfile>();
                cfg.AddProfile<InsertContractItemToContractItemProfile>();
                cfg.AddProfile<InsertContractRenewToContractRenewProfile>();
                cfg.AddProfile<UserSettingsProfile>();
                cfg.AddProfile<ClientViewModelToClientQueryData>();
                cfg.AddProfile<ClientToDetailTabViewModel>();
                cfg.AddProfile<WorkerBossToPersonalDataViewModel>();
                cfg.AddProfile<WorkerBossToWorkerDataViewModel>();
                cfg.AddProfile<WorkerBossToLoginPrivilegesData>();
                cfg.AddProfile<SaleViewModelToContractItemQueryData>();
                cfg.AddProfile<CommodityViewModelToContractItemQueryData>();
                cfg.AddProfile<ContractItemToSaleBasicInfoViewModel>();
                cfg.AddProfile<SaleToSaleBasicInfoViewModel>();
                cfg.AddProfile<SaleToSaleInfoViewModel>();
                cfg.AddProfile<SellToSellDialogViewModel>();
            });
            var mapper = configuration.CreateMapper();

            containerRegistry.RegisterInstance(typeof(IMapper), mapper);
        }
    }
}
