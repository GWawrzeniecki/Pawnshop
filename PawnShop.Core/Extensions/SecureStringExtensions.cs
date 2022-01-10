using System;
using System.Runtime.InteropServices;
using System.Security;

namespace PawnShop.Core.Extensions
{
    public static class SecureStringExtensions
    {
        public static string ConvertToString(this SecureString secureString)
        {
            if (secureString is null)
                throw new ArgumentNullException(nameof(secureString));

            var ptr = Marshal.SecureStringToBSTR(secureString);

            try
            {
                return Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.FreeBSTR(ptr);
            }
        }
    }
}