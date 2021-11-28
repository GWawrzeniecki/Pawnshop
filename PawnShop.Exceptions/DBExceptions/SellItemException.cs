using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class SellItemException : Exception
    {
        public SellItemException()
        {
        }

        public SellItemException(string message) : base(message)
        {
        }

        public SellItemException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}