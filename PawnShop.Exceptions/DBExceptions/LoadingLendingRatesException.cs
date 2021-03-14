using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Exceptions.DBExceptions
{
    public class LoadingLendingRatesException : Exception
    {
        public LoadingLendingRatesException()
        {
        }

        public LoadingLendingRatesException(string message) : base(message)
        {
        }

        public LoadingLendingRatesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
