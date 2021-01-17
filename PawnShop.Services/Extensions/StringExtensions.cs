using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

namespace PawnShop.Services.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<byte> HexStringToByte(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (value.Split(' ').Count() < 0)
                throw new FormatException("Hex values should be separated by space separator");

            if (value.Split(' ').Any(hex => !int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out int result)))
                throw new FormatException("Provided key was not in hex format");

            return value
                .Split(' ')
                .Select(hexValue => Convert.ToByte(hexValue, 16))
                .ToArray();
        }
    }
}
