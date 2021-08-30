using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class DeletingLendingRateException : Exception
    {
        public DeletingLendingRateException()
        {
        }

        public DeletingLendingRateException(string message) : base(message)
        {
        }

        public DeletingLendingRateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}