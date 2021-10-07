using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using PawnShop.Modules.Worker.Base;
using PawnShop.Services.DataService;
using Prism.Mvvm;
using System.Collections.Generic;

namespace PawnShop.Modules.Worker.RegionContext
{
    public class WorkerTabControlRegionContext : BindableBase
    {
        private WorkerBoss _workerBoss;
        private WorkerDialogMode _workerDialogMode;
        private IList<WorkerDialogViewBase> _editViews;
        private IUnitOfWork _unitOfWork;

        public WorkerTabControlRegionContext()
        {
            EditViews = new List<WorkerDialogViewBase>(3);
        }

        public WorkerBoss WorkerBoss
        {
            get => _workerBoss;
            set => SetProperty(ref _workerBoss, value);
        }

        public WorkerDialogMode WorkerDialogMode
        {
            get => _workerDialogMode;
            set => SetProperty(ref _workerDialogMode, value);
        }

        public IList<WorkerDialogViewBase> EditViews
        {
            get => _editViews;
            set => SetProperty(ref _editViews, value);
        }

        public IUnitOfWork UnitOfWork
        {
            get => _unitOfWork;
            set => SetProperty(ref _unitOfWork, value);
        }
    }
}