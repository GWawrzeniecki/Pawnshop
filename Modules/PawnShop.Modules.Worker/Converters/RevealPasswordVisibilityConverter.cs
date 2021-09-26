using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PawnShop.Modules.Worker.Converters
{
    public class RevealPasswordVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[0] is bool shouldBeVisible && values[1] is string text
                ? shouldBeVisible && !string.IsNullOrEmpty(text) ? Visibility.Visible : Visibility.Hidden
                : Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}