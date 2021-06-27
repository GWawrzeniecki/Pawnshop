using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class CreateContractException : Exception
    {
        public CreateContractException()
        {
        }

        public CreateContractException(string message) : base(message)
        {
        }

        public CreateContractException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}