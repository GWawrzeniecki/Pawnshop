using AutoMapper;
using MahApps.Metro.Controls;
using Pawnshop.Setup.Scripts;
using PawnShop.Controls.BaseTaskbar.Views;
using PawnShop.Controls.ContractItemViews.ViewModels;
using PawnShop.Controls.Dialogs.ViewModels;
using PawnShop.Controls.Dialogs.Views;
using PawnShop.Controls.SharedViews.ViewModels;
using PawnShop.Controls.Validators;
using PawnShop.Core;
using PawnShop.Core.Constants;
using PawnShop.Core.Extensions;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Core.SharedVariables;
using PawnShop.Core.Taskbar;
using PawnShop.Dialogs.Views;
using PawnShop.Dialogs.ViewsModels;
using PawnShop.Mapper.Profiles;
using PawnShop.Modules.Client;
using PawnShop.Modules.Commodity;
using PawnShop.Modules.Contract;
using PawnShop.Modules.Home;
using PawnShop.Modules.Login;
using PawnShop.Modules.Login.ViewModels;
using PawnShop.Modules.Login.Views;
using PawnShop.Modules.Sale;
using PawnShop.Modules.Settings;
using PawnShop.Modules.Worker;
using PawnShop.Services.DataService;
using PawnShop.Services.Implementations;
using PawnShop.Services.Interfaces;
using PawnShop.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace PawnShop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IApplicationCommands, ApplicationCommands>();
            containerRegistry.Register<IUnitOfWork, UnitOfWork>();
            containerRegistry.Register<IUIService, UIService>();
            containerRegistry.Register<IShellService, ShellService>();
            containerRegistry.RegisterSingleton<ISessionContext, SessionContext>();
            containerRegistry.RegisterSingleton<IConfigData, ConfigData>();
            containerRegistry.RegisterSingleton<IUserSettings, UserSettings>();
            containerRegistry.Register<IValidatorService, ValidatorService>();
            containerRegistry.Register<IClientService, ClientService>();
            containerRegistry.Register<IContractItemService, ContractItemService>();
            containerRegistry.Register<IConfigurationService, ConfigurationService>();
            containerRegistry.Register<IPrintService, PrintService>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IMessageBoxService, MessageBoxService>();
            containerRegistry.Register<ISetup, Setup>();
            containerRegistry.Register<AddClientValidator>();
            containerRegistry.Register<IPdfService, PdfService>();
            containerRegistry.RegisterInstance<ISettingsService<UserSettings>>(new SettingsService<UserSettings>(Constants.UserSettingsFileName));
            containerRegistry.RegisterDialogWindow<MahappsDialogWindow>();
            containerRegistry.RegisterDialog<LoginDialog, LoginDialogViewModel>();
            containerRegistry.RegisterDialog<NotificationDialog, NotificationDialogViewModel>();
            containerRegistry.RegisterDialog<AddClientDialog, AddClientDialogViewModel>();
            containerRegistry.RegisterForNavigation<Controls.ContractItemViews.Views.Laptop, LaptopViewModel>();
            containerRegistry.RegisterForNavigation<Controls.SharedViews.Views.SaleBaseInfo, SaleBasicInfoViewModel>();
            containerRegistry.RegisterForNavigation<Controls.SharedViews.Views.SaleInfo, SaleInfoViewModel>();
            ConfigureMapper(containerRegistry);
        }

        private static void ConfigureMapper(IContainerRegistry containerRegistry)
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
                cfg.AddProfile<ContractViewModelToContractQueryData>();
            });
            var mapper = configuration.CreateMapper();

            containerRegistry.RegisterInstance(typeof(IMapper), mapper);
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);

            regionAdapterMappings.RegisterMapping<StackPanel>(Container.Resolve<StackPanelRegionAdapter>());
            regionAdapterMappings.RegisterMapping<HamburgerMenuItemCollection>(
                Container.Resolve<HamburgerMenuItemCollectionRegionAdapter>());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<LoginModule>();
            moduleCatalog.AddModule<HomeModule>(InitializationMode.OnDemand);
            moduleCatalog.AddModule<ContractModule>(InitializationMode.OnDemand);
            moduleCatalog.AddModule<SettingsModule>(InitializationMode.OnDemand);
            moduleCatalog.AddModule<ClientModule>(InitializationMode.OnDemand);
            moduleCatalog.AddModule<WorkerModule>(InitializationMode.OnDemand);
            moduleCatalog.AddModule<SaleModule>(InitializationMode.OnDemand);
            moduleCatalog.AddModule<CommodityModule>(InitializationMode.OnDemand);
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            regionBehaviors.AddIfMissing(DependentViewRegionBehavior.BehaviorKey, typeof(DependentViewRegionBehavior));
            regionBehaviors.AddIfMissing(RegionManagerAwareBehavior.BehaviorKey, typeof(RegionManagerAwareBehavior));


            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
        }

        protected override void OnInitialized()
        {
            #region Login

            var loginService = Container.Resolve<ILoginService>();
            var moduleManager = Container.Resolve<IModuleManager>();
            var sessionContext = Container.Resolve<ISessionContext>();
            var result = loginService.ShowLoginDialog();

            if (result != ILoginService.LoginResult.Success) return;
            base.OnInitialized();
            InitializeCulture();
            #region loading modules

            LoadModules(moduleManager, sessionContext);

            #endregion loading modules

            #region registering views

            var regionManager = Container.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion<BottomInfoLine>(RegionNames.BottomInfoLineRegion);

            #endregion registering views

            #endregion Login
        }
        private void LoadModules(IModuleManager moduleManager, ISessionContext sessionContext)
        {
            foreach (var moduleInfo in moduleManager.Modules
                .Where(m => m.InitializationMode == InitializationMode.OnDemand && m.HasCurrentUserPrivilege(sessionContext))
                .OrderModules())
            {
                moduleManager.LoadModule(moduleInfo.ModuleName);
            }
        }

        private void InitializeCulture()
        {
            try
            {
                var polishCulture = new CultureInfo("pl-PL");
                CultureInfo.DefaultThreadCurrentCulture = polishCulture;
                CultureInfo.DefaultThreadCurrentUICulture = polishCulture;

                var lang = XmlLanguage.GetLanguage(polishCulture.IetfLanguageTag);
                FrameworkElement.LanguageProperty.OverrideMetadata(
                    typeof(FrameworkElement),
                    new FrameworkPropertyMetadata(lang)
                );
                //<run> elements
                FrameworkContentElement.LanguageProperty.OverrideMetadata(
                    typeof(TextElement),
                    new FrameworkPropertyMetadata(lang)
                );
            }
            catch (Exception)
            {

            }
        }

    }
}