using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Core.ViewModel;
using PawnShop.Modules.Worker.RegionContext;
using Prism.Mvvm;

namespace PawnShop.Modules.Worker.Base
{
    public abstract class WorkerDialogBase : BindableBase, ITabItemViewModel
    {
        #region PrivateMembers

        private WorkerBoss _workerBoss;
        private WorkerTabControlRegionContext _workerTabControlRegionContext;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        protected WorkerDialogBase(IMapper mapper)
        {
            _mapper = mapper;
        }

        #endregion

        #region PublicProperties

        public WorkerTabControlRegionContext WorkerTabControlRegionContext
        {
            get => _workerTabControlRegionContext;
            set
            {
                SetProperty(ref _workerTabControlRegionContext, value);
                if (value?.WorkerBoss != null)
                    MapWorkerBossToVm();
            }
        }

        #endregion

        #region ITabItemViewModel

        public string Header { get; set; }

        #endregion

        #region PublicMethods
        public virtual void MapVmToWorkerBoss()
        {
            _mapper.Map(this, WorkerTabControlRegionContext.WorkerBoss);
        }

        #endregion

        #region ProtectedMethpods
        protected virtual void MapWorkerBossToVm()
        {
            _mapper.Map(WorkerTabControlRegionContext.WorkerBoss, this);
        }

        #endregion
    }
}