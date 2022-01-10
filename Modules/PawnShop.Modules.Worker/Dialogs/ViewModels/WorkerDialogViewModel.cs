using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Worker.Base;
using PawnShop.Modules.Worker.RegionContext;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PawnShop.Modules.Worker.Dialogs.ViewModels
{
    public class WorkerDialogViewModel : BindableBase, IDialogAware
    {
        #region PrivateMembers

        private string _title;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageBoxService _messageBoxService;
        private WorkerTabControlRegionContext _workerTabControlRegionContext;
        private Visibility _createWorkerButtonVisibility;
        private Visibility _updateWorkerButtonVisibility;
        private Visibility _cancelWorkerButtonVisibility;
        private DelegateCommand _createWorkerCommand;
        private DelegateCommand _updateWorkerCommand;
        private DelegateCommand _cancelCommand;

        #endregion

        #region Constructor

        public WorkerDialogViewModel(IUnitOfWork unitOfWork, IMessageBoxService messageBoxService)
        {
            _unitOfWork = unitOfWork;
            _messageBoxService = messageBoxService;
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

        public Visibility CreateWorkerButtonVisibility
        {
            get => _createWorkerButtonVisibility;
            set => SetProperty(ref _createWorkerButtonVisibility, value);
        }


        public Visibility UpdateWorkerButtonVisibility
        {
            get => _updateWorkerButtonVisibility;
            set => SetProperty(ref _updateWorkerButtonVisibility, value);
        }

        public Visibility CancelWorkerButtonVisibility
        {
            get => _cancelWorkerButtonVisibility;
            set => SetProperty(ref _cancelWorkerButtonVisibility, value);
        }

        #endregion

        #region IDialogAware

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            _unitOfWork.Dispose();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Title = parameters.GetValue<string>("title") ?? string.Empty;
            var workerDialogMode = parameters.GetValue<WorkerDialogMode>("workerDialogMode");
            var workerBoss = parameters.GetValue<WorkerBoss>("workerBoss") ?? new WorkerBoss()
            {
                Privilege = new Privilege(),
                WorkerBossNavigation = new Person(),
            };
            WorkerTabControlRegionContext = new WorkerTabControlRegionContext
            { WorkerBoss = workerBoss, WorkerDialogMode = workerDialogMode, UnitOfWork = _unitOfWork };
            OnMode(workerDialogMode);
        }

        public event Action<IDialogResult> RequestClose;

        #endregion IDialogAware

        #region Commands

        public DelegateCommand CreateWorkerCommand =>
            _createWorkerCommand ??= new DelegateCommand(CreateWorkerAsync, CanExecuteCreateOrUpDateWorker);

        public DelegateCommand CancelCommand =>
            _cancelCommand ??= new DelegateCommand(Cancel);

        public DelegateCommand UpdateWorkerCommand =>
            _updateWorkerCommand ??= new DelegateCommand(UpdateWorkerAsync, CanExecuteCreateOrUpDateWorker);


        #endregion

        #region CommandsMethods

        private async void CreateWorkerAsync()
        {

            try
            {
                await TryToCreateWorkerAsync();
                _messageBoxService.Show("Pracownik zostal pomyslnie dodany.", "Sukces");
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (CreateWorkerException e)
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
        }

        private async void UpdateWorkerAsync()
        {
            try
            {
                await TryToUpdateWorkerAsync();
                _messageBoxService.Show("Pomyślnie zapisano zmiany.", "Sukces");
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (UpdateWorkerException e)
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
        }

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        private bool CanExecuteCreateOrUpDateWorker()
        {
            if (WorkerTabControlRegionContext is null)
                return false;

            return WorkerTabControlRegionContext.EditViews.All(v => (v.DataContext as WorkerDialogBase).IsValidForm);
        }

        #endregion

        #region PrivateMethods

        private void OnMode(WorkerDialogMode workerDialogMode)
        {
            switch (workerDialogMode)
            {
                case WorkerDialogMode.Show:
                    HideAllButtons();
                    break;
                case WorkerDialogMode.Add:
                    HideUpdateButton();
                    break;
                case WorkerDialogMode.Edit:
                    HideCreateButton();
                    break;
                default:
                    break;
            }
        }
        private void HideAllButtons()
        {
            CreateWorkerButtonVisibility = Visibility.Hidden;
            UpdateWorkerButtonVisibility = Visibility.Hidden;
            CancelWorkerButtonVisibility = Visibility.Hidden;
        }

        private void HideUpdateButton()
        {
            UpdateWorkerButtonVisibility = Visibility.Hidden;
        }

        private void HideCreateButton()
        {
            CreateWorkerButtonVisibility = Visibility.Hidden;
        }

        private async Task TryToCreateWorkerAsync()
        {
            try
            {
                AttachAdditionalContexts();
                AttachWorkerBoss();
                MapAllDataToWorkerBoss();
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CreateWorkerException("Wystąpił błąd podczas dodawania pracownika.", e);
            }
        }

        private async Task TryToUpdateWorkerAsync()
        {
            try
            {
                AttachAdditionalContexts();
                AttachWorkerBoss();
                MapAllDataToWorkerBoss();
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new UpdateWorkerException("Wystąpił błąd podczas aktualizowania danych pracownika.", e);
            }
        }
        private void AttachWorkerBoss()
        {
            _unitOfWork.WorkerBossRepository.Attach(WorkerTabControlRegionContext.WorkerBoss);
        }

        private void MapAllDataToWorkerBoss()
        {
            foreach (var workerDialogViewBase in WorkerTabControlRegionContext.EditViews)
            {
                (workerDialogViewBase.DataContext as WorkerDialogBase).MapVmToWorkerBoss();
            }
        }

        private void AttachAdditionalContexts()
        {
            foreach (var workerDialogViewBase in WorkerTabControlRegionContext.EditViews)
            {
                (workerDialogViewBase.DataContext as WorkerDialogBase).AttachAdditionalContext();
            }
        }

        #endregion
    }
}
