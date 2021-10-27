using PawnShop.Core.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PawnShop.Modules.Commodity.Converters
{
    public class PreviewPutOnSaleDialogModeToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is PreviewPutOnSaleDialogMode.Sale;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}