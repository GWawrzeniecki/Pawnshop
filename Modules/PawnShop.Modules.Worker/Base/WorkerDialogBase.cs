using AutoMapper;
using PawnShop.Core.Enums;
using PawnShop.Core.ViewModel;
using PawnShop.Core.ViewModel.Base;
using PawnShop.Modules.Worker.RegionContext;
using PawnShop.Validator.Base;

namespace PawnShop.Modules.Worker.Base
{
    public abstract class WorkerDialogBase : ViewModelBase<WorkerDialogBase>, ITabItemViewModel
    {
        #region PrivateMembers

        private WorkerTabControlRegionContext _workerTabControlRegionContext;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        protected WorkerDialogBase(IMapper mapper, ValidatorBase<WorkerDialogBase> validator) : base(validator)
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
                if (value?.WorkerBoss != null && value?.WorkerDialogMode != WorkerDialogMode.Add)
                    MapWorkerBossToVm();
            }
        }

        public virtual bool IsValidForm => !HasErrors;

        #endregion

        #region ITabItemViewModel

        public string Header { get; set; }

        #endregion

        #region ViewModelBase

        protected override WorkerDialogBase GetInstance()
        {
            return this;
        }

        #endregion

        #region PublicMethods

        public virtual void MapVmToWorkerBoss()
        {
            _mapper.Map(this, WorkerTabControlRegionContext.WorkerBoss);
        }

        public virtual void AttachAdditionalContext()
        {

        }

        #endregion

        #region ProtectedMethods

        protected virtual void MapWorkerBossToVm()
        {
            _mapper.Map(WorkerTabControlRegionContext.WorkerBoss, this);
        }

        #endregion
    }
}