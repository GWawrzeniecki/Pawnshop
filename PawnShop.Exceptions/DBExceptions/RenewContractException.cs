using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class RenewContractException : Exception
    {
        public RenewContractException()
        {
        }

        public RenewContractException(string message) : base(message)
        {
        }

        public RenewContractException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}