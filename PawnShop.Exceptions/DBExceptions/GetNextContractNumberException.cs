using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class GetNextContractNumberException : Exception
    {
        public GetNextContractNumberException()
        {
        }

        public GetNextContractNumberException(string message) : base(message)
        {
        }

        public GetNextContractNumberException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}