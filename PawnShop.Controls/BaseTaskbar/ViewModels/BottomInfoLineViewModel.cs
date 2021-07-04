using PawnShop.Core.SharedVariables;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using BespokeFusion;
using PawnShop.Core.Events;
using PawnShop.Services.DataService;
using Prism.Events;

namespace PawnShop.Controls.BaseTaskbar.ViewModels
{
    public class BottomInfoLineViewModel : BindableBase  // To do bindeableBase w/wo session context
    {

        #region private methods
        private readonly IUnitOfWork _unitOfWork;
        private readonly DispatcherTimer _dispatcherTimer;
        private DateTime _actualDateTime;
        private ISessionContext _sessionContext;

        #endregion private methods

        #region constructor

        public BottomInfoLineViewModel(ISessionContext sessionContext, IEventAggregator eventAggregator, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dispatcherTimer = new DispatcherTimer();
            UpdateActualDateTime();
            SessionContext = sessionContext;
            eventAggregator.GetEvent<MoneyBalanceChangedEvent>().Subscribe(MoneyBalanceChanged);
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
            catch (Exception e)
            {
                MaterialMessageBox.Show($"Nie udało się odświeżyć stanu kasy.{Environment.NewLine}Uruchom ponownie aplikacje.", "Błąd");

            }
        }

        public async Task TryToUpdateMoneyBalanceAsync()
        {
            _sessionContext.TodayMoneyBalance = await _unitOfWork.MoneyBalanceRepository.GetTodayMoneyBalanceAsync();
        }

        #endregion
    }
}