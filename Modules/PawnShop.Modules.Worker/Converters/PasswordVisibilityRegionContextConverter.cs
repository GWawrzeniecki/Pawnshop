using PawnShop.Core.Enums;
using PawnShop.Modules.Worker.RegionContext;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PawnShop.Modules.Worker.Converters
{
    public class PasswordVisibilityRegionContextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is WorkerTabControlRegionContext workerTabControlRegionContext
                ? workerTabControlRegionContext.WorkerDialogMode == WorkerDialogMode.Show ? Visibility.Hidden :
                Visibility.Visible
                : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}