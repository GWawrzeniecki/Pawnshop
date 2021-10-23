using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class LoadingContractItemsException : Exception
    {
        public LoadingContractItemsException()
        {
        }

        public LoadingContractItemsException(string message) : base(message)
        {
        }

        public LoadingContractItemsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}