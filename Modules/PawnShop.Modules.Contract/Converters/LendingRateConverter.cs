using PawnShop.Business.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PawnShop.Modules.Contract.Converters
{
    public class LendingRateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is LendingRate lendingRate ? lendingRate.Days : null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.TryParse(value?.ToString(), out var digit) ? new LendingRate { Days = digit } : null;
        }
    }
}