using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class BuyBackContractException : Exception
    {
        public BuyBackContractException()
        {
        }

        public BuyBackContractException(string message) : base(message)
        {
        }

        public BuyBackContractException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}