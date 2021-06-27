using System;

namespace PawnShop.Exceptions
{
    public class PrintDealDocumentException : Exception
    {
        public PrintDealDocumentException()
        {
        }

        public PrintDealDocumentException(string message) : base(message)
        {
        }

        public PrintDealDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
