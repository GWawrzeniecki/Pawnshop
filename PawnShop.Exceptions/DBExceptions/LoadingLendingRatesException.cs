using System;

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
