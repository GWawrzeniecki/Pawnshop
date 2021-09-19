using PawnShop.Core.Enums;
using PawnShop.Modules.Worker.RegionContext;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PawnShop.Modules.Worker.Converters
{
    public class WorkerTabControlRegionContextToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is WorkerTabControlRegionContext workerTabControlRegionContext
                ? workerTabControlRegionContext.WorkerDialogMode != WorkerDialogMode.Show
                : (object)true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}