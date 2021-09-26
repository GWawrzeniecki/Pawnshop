﻿using BespokeFusion;
using PawnShop.Business.Models;
using PawnShop.Core.Enums;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Worker.Base;
using PawnShop.Modules.Worker.Dialogs.Views;
using PawnShop.Modules.Worker.RegionContext;
using PawnShop.Services.DataService;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
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
        private readonly IRegionManager _regionManager;
        private readonly IUnitOfWork _unitOfWork;
        private WorkerTabControlRegionContext _workerTabControlRegionContext;
        private Visibility _createWorkerButtonVisibility;
        private Visibility _updateWorkerButtonVisibility;
        private Visibility _cancelWorkerButtonVisibility;
        private Visibility _buttonGridButtonVisibility;
        private DelegateCommand _createWorkerCommand;
        private DelegateCommand _updateWorkerCommand;

        #endregion

        #region Constructor

        public WorkerDialogViewModel(IRegionManager regionManager, IUnitOfWork unitOfWork)
        {
            _regionManager = regionManager;
            _unitOfWork = unitOfWork;
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

        public Visibility ButtonGridButtonVisibility
        {
            get => _buttonGridButtonVisibility;
            set => SetProperty(ref _buttonGridButtonVisibility, value);
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
            var workerBoss = parameters.GetValue<WorkerBoss>("workerBoss") ?? new WorkerBoss();
            WorkerTabControlRegionContext = new WorkerTabControlRegionContext
            { WorkerBoss = workerBoss, WorkerDialogMode = workerDialogMode };
            OnMode(workerDialogMode);

        }

        public event Action<IDialogResult> RequestClose;

        #endregion IDialogAware

        #region Commands

        public DelegateCommand CreateWorkerCommand =>
            _createWorkerCommand ??= new DelegateCommand(CreateWorker);



        public DelegateCommand UpdateWorkerCommand =>
            _updateWorkerCommand ??= new DelegateCommand(UpdateWorkerAsync);


        #endregion

        #region CommandsMethods

        private void CreateWorker()
        {

        }

        private async void UpdateWorkerAsync()
        {
            try
            {
                await TryToUpdateWorkerAsync();
                MaterialMessageBox.Show("Pomyślnie zapisano zmiany.", "Sukces");
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (UpdateClientException e)
            {
                MaterialMessageBox.ShowError(
                    $"{e.Message}.{Environment.NewLine}Błąd: {e.InnerException?.Message}",
                    "Błąd");
            }
            catch (Exception e)
            {

                MaterialMessageBox.ShowError(
                    $"Ups.. coś poszło nie tak.{Environment.NewLine}Błąd: {e.Message}",
                    "Błąd");
            }
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
                    break;
                case WorkerDialogMode.Edit:
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

        private async Task TryToUpdateWorkerAsync()
        {
            try
            {
                AttachWorkerBoss();
                AttachCountriesBasedOnMode();
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

        private void AttachCountriesBasedOnMode()
        {
            var personalDataView = WorkerTabControlRegionContext.EditViews.First(view => view is PersonalData);
            var Countries = (personalDataView.DataContext as PersonalDataViewModel).Countries;

            if (WorkerTabControlRegionContext.WorkerDialogMode == WorkerDialogMode.Add)
            {
                foreach (var country in Countries)
                {
                    _unitOfWork.CountryRepository.Attach(country);
                }
            }
            else
            {
                foreach (var country in Countries.Where(c => c.CountryId != WorkerTabControlRegionContext.WorkerBoss.WorkerBossNavigation.Address.Country.CountryId))
                {
                    _unitOfWork.CountryRepository.Attach(country);
                }
            }

        }

        #endregion
    }
}
