using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class InsertSaleException : Exception
    {
        public InsertSaleException()
        {
        }

        public InsertSaleException(string message) : base(message)
        {
        }

        public InsertSaleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}