using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using Prism.Mvvm;

namespace PawnShop.Modules.Worker.RegionContext
{
    public class WorkerTabControlRegionContext : BindableBase
    {
        private WorkerBoss _workerBoss;
        private WorkerDialogMode _workerDialogMode;

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
    }
}