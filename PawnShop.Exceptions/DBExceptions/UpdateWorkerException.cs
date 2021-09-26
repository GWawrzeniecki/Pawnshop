using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class UpdateWorkerException : Exception
    {
        public UpdateWorkerException()
        {
        }

        public UpdateWorkerException(string message) : base(message)
        {
        }

        public UpdateWorkerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}