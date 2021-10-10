using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class DeleteWorkerException : Exception
    {
        public DeleteWorkerException()
        {

        }

        public DeleteWorkerException(string message) : base(message)
        {

        }

        public DeleteWorkerException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}