using PawnShop.Modules.Worker.RegionContext;
using Prism.Common;
using System.ComponentModel;
using System.Windows.Controls;

namespace PawnShop.Modules.Worker.Base
{
    public class WorkerDialogViewBase : UserControl // has to be not abstract because of x:Name
    {
        private WorkerTabControlRegionContext _workerTabControlRegionContext;

        public WorkerDialogViewBase()
        {
            ObserveWorkerContext();
        }

        private void ObserveWorkerContext()
        {
            ObservableObject<object> viewRegionContext = Prism.Regions.RegionContext.GetObservableContext(this);
            viewRegionContext.PropertyChanged += ViewRegionContext_OnPropertyChangedEvent;
        }

        private void ViewRegionContext_OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;
            var workerTabControlRegionContext = context.Value as WorkerTabControlRegionContext;
            _workerTabControlRegionContext = workerTabControlRegionContext;
            PassRegionContextToDataContext();
            RegisterInRegionContext();
        }

        private void PassRegionContextToDataContext()
        {
            if (DataContext is WorkerDialogBase workerDialog)
                workerDialog.WorkerTabControlRegionContext = _workerTabControlRegionContext;
        }

        private void RegisterInRegionContext()
        {
            _workerTabControlRegionContext.EditViews.Add(this);
        }


    }
}