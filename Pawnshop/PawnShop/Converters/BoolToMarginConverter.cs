using System;
using System.Globalization;
using System.Windows.Data;

namespace PawnShop.Converters
{
    internal class BoolToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isPaneOpen)
            {
                if (isPaneOpen)
                    return new System.Windows.Thickness(150, 0, 0, 0);
                return new System.Windows.Thickness(50, 0, 0, 0);
            }
            else
            {
                return new System.Windows.Thickness(0);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}