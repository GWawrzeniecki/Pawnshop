using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class LoadingClientsException : Exception
    {
        public LoadingClientsException()
        {
        }

        public LoadingClientsException(string message) : base(message)
        {
        }

        public LoadingClientsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}