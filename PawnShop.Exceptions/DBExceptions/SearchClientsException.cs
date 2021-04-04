using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class SearchClientsException : Exception
    {

        public SearchClientsException()
        {
        }

        public SearchClientsException(string message) : base(message)
        {
        }

        public SearchClientsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}