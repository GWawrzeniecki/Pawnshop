using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class LoadingStartupDataException : Exception
    {
        public LoadingStartupDataException()
        {
        }

        public LoadingStartupDataException(string message) : base(message)
        {
        }

        public LoadingStartupDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}