using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class EditingLendingRateException : Exception
    {
        public EditingLendingRateException()
        {
        }

        public EditingLendingRateException(string message) : base(message)
        {
        }

        public EditingLendingRateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}