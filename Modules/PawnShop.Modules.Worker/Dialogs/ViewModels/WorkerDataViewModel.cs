using AutoMapper;
using BespokeFusion;
using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using PawnShop.Modules.Worker.Base;
using PawnShop.Services.DataService;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Worker.Dialogs.ViewModels
{
    public class WorkerDataViewModel : WorkerDialogBase
    {
        #region PrivateMembers

        private string _workerBossTypeStr;
        private DateTime _hireDate;
        private DateTime _datePhysicalCheckUp;
        private int? _salary;
        private int? _grantedBonus;
        private IList<WorkerBossType> _workerBossTypes;
        private WorkerBossType _selectedWorkerBossType;
        private readonly IContainerProvider _containerProvider;
        private Task _loadWorkerBossTypesTask;

        #endregion

        #region Constructor

        public WorkerDataViewModel(IMapper mapper, IContainerProvider containerProvider) : base(mapper)
        {
            _containerProvider = containerProvider;
            Header = "Dane pracownika";
            LoadWorkerBossTypes();
        }

        #endregion

        #region PublicProperties

        public string WorkerBossTypeStr // better bname
        {
            get => _workerBossTypeStr;
            set => SetProperty(ref _workerBossTypeStr, value);
        }

        public IList<WorkerBossType> WorkerBossTypes
        {
            get => _workerBossTypes;
            set => SetProperty(ref _workerBossTypes, value);
        }

        public WorkerBossType SelectedWorkerBossType
        {
            get => _selectedWorkerBossType;
            set => SetProperty(ref _selectedWorkerBossType, value);
        }

        public DateTime HireDate
        {
            get => _hireDate;
            set => SetProperty(ref _hireDate, value);
        }

        public DateTime DatePhysicalCheckUp
        {
            get => _datePhysicalCheckUp;
            set => SetProperty(ref _datePhysicalCheckUp, value);
        }

        public int? Salary
        {
            get => _salary;
            set => SetProperty(ref _salary, value);
        }

        public int? GrantedBonus
        {
            get => _grantedBonus;
            set => SetProperty(ref _grantedBonus, value);
        }

        #endregion

        #region WorkerDialogBase

        protected override void MapWorkerBossToVm()
        {
            base.MapWorkerBossToVm();
            MapWorkerBossTypeBasedOnMode();
        }

        #endregion

        #region PrivateMethods

        private void LoadWorkerBossTypes()
        {
            try
            {
                _loadWorkerBossTypesTask = TryToLoadWorkerBossTypesAsync();
            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Wystąpił problem podczas pobierania rodzajów pracownika.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private async Task TryToLoadWorkerBossTypesAsync()
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            WorkerBossTypes = (await unitOfWork.WorkerBossTypeRepository.GetAsync()).ToList();
        }

        private async void MapWorkerBossTypeBasedOnMode()
        {
            switch (WorkerTabControlRegionContext.WorkerDialogMode)
            {
                case WorkerDialogMode.Show:
                case WorkerDialogMode.Edit:
                    await _loadWorkerBossTypesTask;
                    ApplyWorkerBossTypeToRegionContext();
                    MapWorkerBossTypeToVm();
                    break;
            }
        }

        private async void ApplyWorkerBossTypeToRegionContext()
        {
            await _loadWorkerBossTypesTask;
            WorkerTabControlRegionContext.WorkerBoss.WorkerBossType = WorkerBossTypes.First(w =>
                w.Id == WorkerTabControlRegionContext.WorkerBoss.WorkerBossTypeId);
        }

        private void MapWorkerBossTypeToVm()
        {
            SelectedWorkerBossType = WorkerTabControlRegionContext.WorkerBoss.WorkerBossType;
        }


        #endregion
    }
}
