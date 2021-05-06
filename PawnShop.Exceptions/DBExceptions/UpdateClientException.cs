using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class UpdateClientException : Exception
    {
        public UpdateClientException()
        {
        }

        public UpdateClientException(string message) : base(message)
        {
        }

        public UpdateClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}