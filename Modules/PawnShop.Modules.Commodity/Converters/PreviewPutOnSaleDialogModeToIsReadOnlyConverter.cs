using PawnShop.Core.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PawnShop.Modules.Commodity.Converters
{
    public class PreviewPutOnSaleDialogModeToIsReadOnlyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is not PreviewPutOnSaleDialogMode dialogMode ||
                   (dialogMode == PreviewPutOnSaleDialogMode.Preview);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}