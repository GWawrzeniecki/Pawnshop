using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Exceptions.DBExceptions
{
    public class LoadingContractsException : Exception
    {
        public LoadingContractsException()
        {
        }

        public LoadingContractsException(string message) : base(message)
        {
        }

        public LoadingContractsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
