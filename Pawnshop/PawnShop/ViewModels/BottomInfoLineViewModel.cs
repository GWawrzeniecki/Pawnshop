using PawnShop.Core.SharedVariables;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace PawnShop.ViewModels
{
    public class BottomInfoLineViewModel : BindableBase  // To do bindeableBase w/wo session context
    {

        #region private methods

        DispatcherTimer _dispatcherTimer;
        private DateTime _actualDateTime;
        private ISessionContext _sessionContext;
        #endregion

        #region constructor
        public BottomInfoLineViewModel(ISessionContext sessionContext)
        {
            _dispatcherTimer = new DispatcherTimer();
            UpdateActualDateTime();
            SessionContext = sessionContext;
        }

        #endregion

        #region public properties


        public DateTime ActualDateTime
        {
            get { return _actualDateTime; }
            set { SetProperty(ref _actualDateTime, value); }
        }


        public ISessionContext SessionContext
        {
            get { return _sessionContext; }
            set { SetProperty(ref _sessionContext, value); }
        }

        public string FullName => $"{SessionContext.LoggedPerson?.FirstName} {SessionContext.LoggedPerson?.LastName}"; // to do ..

        #endregion


        #region private methods

        private void UpdateActualDateTime()
        {
            _dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            _dispatcherTimer.Tick += DispatcherTimerTick;
            _dispatcherTimer.Start();
        }

        #endregion

        #region dispatcher event

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            ActualDateTime = DateTime.Now;
        }

        #endregion


    }
}
