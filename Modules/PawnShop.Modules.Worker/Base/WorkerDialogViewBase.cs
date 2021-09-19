using PawnShop.Modules.Worker.RegionContext;
using Prism.Common;
using System.ComponentModel;
using System.Windows.Controls;

namespace PawnShop.Modules.Worker.Base
{
    public class WorkerDialogViewBase : UserControl
    {
        public WorkerDialogViewBase()
        {
            ObserveWorkerContext();
        }

        private void ObserveWorkerContext()
        {
            ObservableObject<object> viewRegionContext = Prism.Regions.RegionContext.GetObservableContext(this);
            viewRegionContext.PropertyChanged += this.ViewRegionContext_OnPropertyChangedEvent;
        }

        private void ViewRegionContext_OnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;
            var workerTabControlRegionContext = context.Value as WorkerTabControlRegionContext;
            if (DataContext is WorkerDialogBase workerDialog)
                workerDialog.WorkerTabControlRegionContext = workerTabControlRegionContext;
        }


    }
}