using AutoMapper;
using PawnShop.Modules.Worker.Base;
using System;

namespace PawnShop.Modules.Worker.Dialogs.ViewModels
{
    public class WorkerDataViewModel : WorkerDialogBase
    {
        #region PrivateMembers

        private string _workerBossType;
        private DateTime _hireDate;
        private DateTime _datePhysicalCheckUp;
        private int? _salary;
        private int? _grantedBonus;

        #endregion

        #region Constructor

        public WorkerDataViewModel(IMapper mapper) : base(mapper)
        {
            Header = "Dane pracownika";
        }

        #endregion

        #region PublicProperties

        public string WorkerBossType
        {
            get => _workerBossType;
            set => SetProperty(ref _workerBossType, value);
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


    }
}
