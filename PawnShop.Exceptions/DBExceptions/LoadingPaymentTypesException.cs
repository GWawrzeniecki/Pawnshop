using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class LoadingPaymentTypesException : Exception
    {
        public LoadingPaymentTypesException()
        {
        }

        public LoadingPaymentTypesException(string message) : base(message)
        {
        }

        public LoadingPaymentTypesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}