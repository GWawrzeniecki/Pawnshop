using PawnShop.Core.SharedVariables;
using Prism.Mvvm;
using System;
using System.Windows.Threading;

namespace PawnShop.Controls.BaseTaskbar.ViewModels
{
    public class BottomInfoLineViewModel : BindableBase  // To do bindeableBase w/wo session context
    {
        #region private methods

        private DispatcherTimer _dispatcherTimer;
        private DateTime _actualDateTime;
        private ISessionContext _sessionContext;

        #endregion private methods

        #region constructor

        public BottomInfoLineViewModel(ISessionContext sessionContext)
        {
            _dispatcherTimer = new DispatcherTimer();
            UpdateActualDateTime();
            SessionContext = sessionContext;
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

        public string FullName => $"{SessionContext.LoggedPerson.FirstName} {SessionContext.LoggedPerson.LastName}";

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
    }
}