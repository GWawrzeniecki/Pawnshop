using System;
using System.Globalization;
using System.Windows.Data;

namespace PawnShop.Core.Converters
{
    public class AlternationIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is int number ? ++number : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
