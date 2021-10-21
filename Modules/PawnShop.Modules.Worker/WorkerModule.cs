using PawnShop.Core.Attributes;
using PawnShop.Core.Regions;
using PawnShop.Modules.Worker.Dialogs.ViewModels;
using PawnShop.Modules.Worker.Dialogs.Views;
using PawnShop.Modules.Worker.MenuItem;
using PawnShop.Modules.Worker.Validators;
using PawnShop.Modules.Worker.ViewModels;
using PawnShop.Modules.Worker.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PawnShop.Modules.Worker
{
    [Privilege("WorkersTab")]
    [Order(6)]
    public class WorkerModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public WorkerModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.MenuRegion].Add(containerProvider.Resolve<WorkerHamburgerMenuItem>());
            _regionManager.RegisterViewWithRegion<PersonalData>(RegionNames.WorkerTabControlRegion);
            _regionManager.RegisterViewWithRegion<WorkerData>(RegionNames.WorkerTabControlRegion);
            _regionManager.RegisterViewWithRegion<LoginPrivilegesData>(RegionNames.WorkerTabControlRegion);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<WorkerDialog, WorkerDialogViewModel>();
            containerRegistry.RegisterForNavigation<Workers, WorkersViewModel>();
            containerRegistry.RegisterForNavigation<PersonalData, PersonalDataViewModel>();
            containerRegistry.RegisterForNavigation<WorkerData, WorkerDataViewModel>();
            containerRegistry.RegisterForNavigation<LoginPrivilegesData, LoginPrivilegesDataViewModel>();
            containerRegistry.Register<PersonalDataViewModelValidator>();
            containerRegistry.Register<WorkerDataViewModelValidator>();
            containerRegistry.Register<LoginPrivilegesDataViewModelValidator>();
        }
    }
}