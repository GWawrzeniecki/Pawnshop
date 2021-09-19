using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using PawnShop.Modules.Worker.RegionContext;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;

namespace PawnShop.Modules.Worker.Dialogs.ViewModels
{
    public class WorkerDialogViewModel : BindableBase, IDialogAware
    {
        #region PrivateMembers

        private string _title;
        private readonly IRegionManager _regionManager;
        private WorkerTabControlRegionContext _workerTabControlRegionContext;

        #endregion

        #region Constructor

        public WorkerDialogViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        #endregion

        #region PublicProperties

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public WorkerTabControlRegionContext WorkerTabControlRegionContext
        {
            get => _workerTabControlRegionContext;
            set => SetProperty(ref _workerTabControlRegionContext, value);
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
            Title = parameters.GetValue<string>("title") ?? string.Empty;
            var workerDialogMode = parameters.GetValue<WorkerDialogMode>("workerDialogMode");
            var workerBoss = parameters.GetValue<WorkerBoss>("workerBoss") ?? new WorkerBoss();
            WorkerTabControlRegionContext = new WorkerTabControlRegionContext
            { WorkerBoss = workerBoss, WorkerDialogMode = workerDialogMode };
        }

        public event Action<IDialogResult> RequestClose;

        #endregion IDialogAware

        #region PrivateMethods

        #endregion
    }
}
