using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class CreateClientException : Exception
    {
        public CreateClientException()
        {
        }

        public CreateClientException(string message) : base(message)
        {
        }

        public CreateClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}