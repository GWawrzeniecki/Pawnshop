using PawnShop.Exceptions.DBExceptions;
using System;
using System.Security;

namespace PawnShop.Core.Extensions
{
    public static class StringExtensions
    {
        public static string GetNextContractNumber(this string contractNumber)
        {
            if (string.IsNullOrEmpty(contractNumber))
                throw new ArgumentException("Value cannot be null or empty.", nameof(contractNumber));

            if (string.IsNullOrWhiteSpace(contractNumber))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(contractNumber));

            var number = contractNumber.Substring(0, contractNumber.IndexOf("/"));

            if (!int.TryParse(number, out var parsedResult))
                throw new GetNextContractNumberException($"Aktualny numer umowy był nieprawidłowy.{number}");

            var year = contractNumber[(contractNumber.IndexOf("/") + 1)..];

            return parsedResult < 9 ? $"0{++parsedResult}/{year}" : $"{++parsedResult}/{year}";
        }

        public static SecureString ToSecureString(this string str)
        {
            if (string.IsNullOrEmpty(str)) throw new ArgumentException("Value cannot be null or empty.", nameof(str));

            var secureString = new SecureString();
            foreach (var c in str.ToCharArray())
            {
                secureString.AppendChar(c);
            }

            return secureString;
        }
    }
}