using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PawnShop.Services.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<byte> HexStringToByte(this string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException($"'{nameof(value)}' cannot be null or empty.", nameof(value));

            if (value.Split(' ').Count() < 0)
                throw new FormatException("Hex values should be separated by space separator.");

            if (value.Split(' ').Any(hex => !int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out int result)))
                throw new FormatException("Provided value was not in hex format.");

            return value
                .Split(' ')
                .Select(hexValue => Convert.ToByte(hexValue, 16))
                .ToArray();
        }
    }
}