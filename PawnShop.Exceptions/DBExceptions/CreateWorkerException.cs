using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class CreateWorkerException : Exception
    {
        public CreateWorkerException()
        {
        }

        public CreateWorkerException(string message) : base(message)
        {
        }

        public CreateWorkerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}