using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class LoadingContractItemStatesException : Exception
    {
        public LoadingContractItemStatesException()
        {
        }

        public LoadingContractItemStatesException(string message) : base(message)
        {
        }

        public LoadingContractItemStatesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}