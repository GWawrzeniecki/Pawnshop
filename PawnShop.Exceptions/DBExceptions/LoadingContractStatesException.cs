using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class LoadingContractStatesException : Exception
    {
        public LoadingContractStatesException()
        {
        }

        public LoadingContractStatesException(string message) : base(message)
        {
        }

        public LoadingContractStatesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
