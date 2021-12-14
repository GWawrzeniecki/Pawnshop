using PawnShop.Business.Models;
using PawnShop.Core.Dialogs;
using PawnShop.Core.Enums;
using PawnShop.Core.SharedVariables;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
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
        private readonly ISessionContext _sessionContext;
        private readonly IMessageBoxService _messageBoxService;
        private DelegateCommand _showWorkerCommand;
        private DelegateCommand _createWorkerCommand;
        private DelegateCommand _editWorkerCommand;
        private DelegateCommand _deleteWorkerCommand;
        private bool _isBusy;

        #endregion

        #region Constructor

        public WorkersViewModel(IContainerProvider containerProvider, IDialogService dialogService, ISessionContext sessionContext, IMessageBoxService messageBoxService)
        {
            _containerProvider = containerProvider;
            _dialogService = dialogService;
            _sessionContext = sessionContext;
            _messageBoxService = messageBoxService;
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

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
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

        private async void CreateWorker()
        {
            var dialogResult = ButtonResult.Cancel;

            _dialogService.ShowWorkerDialog((result) =>
           {
               if (result.Result == ButtonResult.OK)
               {
                   dialogResult = result.Result;
               }
           }, "Rejestracja nowego pracownika", WorkerDialogMode.Add);

            if (dialogResult == ButtonResult.OK)
            {
                IsBusy = true;
                await RefreshData();
                IsBusy = false;
            }
        }

        private async void EditWorker()
        {
            var dialogResult = ButtonResult.Cancel;

            _dialogService.ShowWorkerDialog((result) =>
           {
               if (result.Result == ButtonResult.OK)
               {
                   dialogResult = result.Result;
               }
           }, "Edycja pracownika", WorkerDialogMode.Edit, SelectedWorkerBoss);

            if (dialogResult == ButtonResult.OK)
            {
                IsBusy = true;
                await RefreshData();
                IsBusy = false;
            }

        }

        private bool CanExecuteShowEditDeleteWorkerBoss()
        {
            return SelectedWorkerBoss is not null;
        }

        private async void DeleteWorker()
        {
            try
            {
                if (_sessionContext.LoggedPerson.WorkerBossId == SelectedWorkerBoss.WorkerBossId)
                {
                    _messageBoxService.ShowError("Nie możesz usunąc aktualnie zalogowanego pracownika.", "Uwaga!");
                }
                else
                {
                    IsBusy = true;
                    await TryToDeleteWorker();
                    _messageBoxService.Show("Pracownik został pomyślnie usunięty.", "Sukces");
                    await RefreshData();
                }
            }
            catch (DeleteWorkerException e)
            {
                _messageBoxService.ShowError(
                    $"{e.Message}.{Environment.NewLine}Błąd: {e.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {

                _messageBoxService.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
            finally
            {
                IsBusy = false;
            }
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
                _messageBoxService.ShowError(
                    $"Wystąpił błąd podczas ładowania listy pracowników.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }
        private async Task RefreshData()
        {
            try
            {
                await TryToLoadStartupData();

            }
            catch (Exception e)
            {
                _messageBoxService.ShowError(
                    $"Wystąpił błąd podczas odświeżania listy pracowników.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
        }

        private async Task TryToLoadStartupData()
        {
            var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            WorkerBosses = await unitOfWork.WorkerBossRepository.GetWorkerBosses();
        }

        private async Task TryToDeleteWorker()
        {
            try
            {
                var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
                unitOfWork.WorkerBossRepository.Delete(SelectedWorkerBoss);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DeleteWorkerException("Wystąpił błąd podczas usuwania pracownika.", e);
            }
        }


        #endregion

    }
}
