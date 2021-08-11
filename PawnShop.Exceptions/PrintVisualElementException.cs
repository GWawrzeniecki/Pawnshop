using System;

namespace PawnShop.Exceptions
{
    public class PrintVisualElementException : Exception
    {
        public PrintVisualElementException()
        {
        }

        public PrintVisualElementException(string message) : base(message)
        {
        }

        public PrintVisualElementException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}