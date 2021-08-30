using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class AddingLendingRateException : Exception
    {
        public AddingLendingRateException()
        {
        }

        public AddingLendingRateException(string message) : base(message)
        {
        }

        public AddingLendingRateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}