using BespokeFusion;
using PawnShop.Business.Models;
using PawnShop.Core.Dialogs;
using PawnShop.Core.Enums;
using PawnShop.Services.DataService;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawnShop.Modules.Worker.ViewModels
{
    public class WorkersViewModel : BindableBase
    {
        #region PrivateMembers

        private IList<WorkerBoss> _workerBosses;
        private WorkerBoss _selectedWorkerBoss;
        private readonly IContainerProvider _containerProvider;
        private readonly IDialogService _dialogService;
        private DelegateCommand _showWorkerCommand;
        private DelegateCommand _createWorkerCommand;
        private DelegateCommand _editWorkerCommand;
        private DelegateCommand _deleteWorkerCommand;

        #endregion

        #region Constructor

        public WorkersViewModel(IContainerProvider containerProvider, IDialogService dialogService)
        {
            _containerProvider = containerProvider;
            _dialogService = dialogService;
            WorkerBosses = new List<WorkerBoss>();
            LoadStartupData();
        }

        #endregion

        #region PublicProperties

        public IList<WorkerBoss> WorkerBosses
        {
            get => _workerBosses;
            set => SetProperty(ref _workerBosses, value);
        }

        public WorkerBoss SelectedWorkerBoss
        {
            get => _selectedWorkerBoss;
            set => SetProperty(ref _selectedWorkerBoss, value);
        }


        #endregion

        #region Commands

        public DelegateCommand ShowWorkerCommand =>
            _showWorkerCommand ??= new DelegateCommand(ShowWorker, CanExecuteShowEditDeleteWorkerBoss)
                .ObservesProperty(() => SelectedWorkerBoss);

        public DelegateCommand CreateWorkerCommand =>
            _createWorkerCommand ??= new DelegateCommand(CreateWorker);

        public DelegateCommand EditWorkerCommand =>
            _editWorkerCommand ??= new DelegateCommand(EditWorker, CanExecuteShowEditDeleteWorkerBoss)
                .ObservesProperty(() => SelectedWorkerBoss);

        public DelegateCommand DeleteWorkerCommand =>
            _deleteWorkerCommand ??= new DelegateCommand(DeleteWorker, CanExecuteShowEditDeleteWorkerBoss)
                .ObservesProperty(() => SelectedWorkerBoss);

        #endregion

        #region CommandsMethods

        private void ShowWorker()
        {
            _dialogService.ShowWorkerDialog(null, "Podgląd pracownika", WorkerDialogMode.Show, SelectedWorkerBoss);


        }

        private void CreateWorker()
        {

        }

        private void EditWorker()
        {
            _dialogService.ShowWorkerDialog((result) =>
            {
                if (result.Result == ButtonResult.OK)
                {

                }
            }, "Edycja pracownika", WorkerDialogMode.Edit, SelectedWorkerBoss);
        }

        private bool CanExecuteShowEditDeleteWorkerBoss()
        {
            return SelectedWorkerBoss is not null;
        }

        private void DeleteWorker()
        {

        }

        #endregion

        #region PrivateMethods

        private async void LoadStartupData()
        {
            try
            {
                await TryToLoadStartupData();

            }
            catch (Exception e)
            {
                MaterialMessageBox.ShowError(
                    $"Wystąpił błąd podczas ładowania listy pracowników.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private async Task TryToLoadStartupData()
        {
            var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            WorkerBosses = await unitOfWork.WorkerBossRepository.GetWorkerBosses();
        }


        #endregion

    }
}
