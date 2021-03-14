using System;
using System.Collections.Generic;
using System.Text;

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
