using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class LoadingSalesException : Exception
    {
        public LoadingSalesException()
        {
        }

        public LoadingSalesException(string message) : base(message)
        {
        }

        public LoadingSalesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}