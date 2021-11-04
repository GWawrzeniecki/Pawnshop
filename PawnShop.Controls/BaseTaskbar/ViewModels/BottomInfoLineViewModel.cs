﻿using BespokeFusion;
using PawnShop.Core.Events;
using PawnShop.Core.SharedVariables;
using PawnShop.Services.DataService;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PawnShop.Controls.BaseTaskbar.ViewModels
{
    public class BottomInfoLineViewModel : BindableBase
    {
        #region private members

        private readonly IContainerProvider _containerProvider;
        private readonly DispatcherTimer _dispatcherTimer;
        private DateTime _actualDateTime;
        private ISessionContext _sessionContext;

        #endregion private members

        #region constructor

        public BottomInfoLineViewModel(ISessionContext sessionContext, IEventAggregator eventAggregator, IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
            _dispatcherTimer = new DispatcherTimer();
            UpdateActualDateTime();
            SessionContext = sessionContext;
            eventAggregator.GetEvent<MoneyBalanceChangedEvent>().Subscribe(MoneyBalanceChanged);
            eventAggregator.GetEvent<UserChangedEvent>().Subscribe(() => RaisePropertyChanged(nameof(FullName)));
        }

        #endregion constructor

        #region public properties

        public DateTime ActualDateTime
        {
            get => _actualDateTime;
            set => SetProperty(ref _actualDateTime, value);
        }

        public ISessionContext SessionContext
        {
            get => _sessionContext;
            set => SetProperty(ref _sessionContext, value);
        }

        public string FullName => $"{SessionContext.LoggedPerson.WorkerBossNavigation?.FirstName} {SessionContext.LoggedPerson.WorkerBossNavigation?.LastName}";

        #endregion public properties

        #region private methods

        private void UpdateActualDateTime()
        {
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            _dispatcherTimer.Tick += DispatcherTimerTick;
            _dispatcherTimer.Start();
        }

        #endregion private methods

        #region dispatcher event

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            ActualDateTime = DateTime.Now;
        }

        #endregion dispatcher event

        #region MoneyBalanceChangedEvent

        private async void MoneyBalanceChanged()
        {
            try
            {
                await TryToUpdateMoneyBalanceAsync();
            }
            catch (Exception)
            {
                MaterialMessageBox.Show($"Nie udało się odświeżyć stanu kasy.{Environment.NewLine}Uruchom ponownie aplikacje.", "Błąd");

            }
        }

        public async Task TryToUpdateMoneyBalanceAsync()
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            _sessionContext.TodayMoneyBalance = await unitOfWork.MoneyBalanceRepository.GetTodayMoneyBalanceAsync();
        }

        #endregion
    }
}