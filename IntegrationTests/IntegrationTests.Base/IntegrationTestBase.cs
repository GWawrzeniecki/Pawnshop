using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PawnShop.Controls.Validators;
using PawnShop.Core;
using PawnShop.Core.SharedVariables;
using PawnShop.DataAccess.Data;
using PawnShop.Mapper.Profiles;
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
            ContainerProvider.Register<IEventAggregator, EventAggregator>();
            ContainerProvider.Register<IRegionManager, RegionManager>();
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
            ContainerProvider.RegisterSingleton<LoginDialogValidator>();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<PawnshopContext>();
            dbContextOptionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["PawnShopDatabaseTests"].ConnectionString);
            ContainerProvider.Register<IUnitOfWork>(() => new UnitOfWork(ContainerProvider, dbContextOptionsBuilder.Options));
            ViewModel = ContainerProvider.Resolve<T>();
            ConfigureMapper(ContainerProvider);
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
