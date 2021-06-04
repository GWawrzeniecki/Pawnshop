using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class UpdatingContractStatesException : Exception
    {
        public UpdatingContractStatesException()
        {
        }

        public UpdatingContractStatesException(string message) : base(message)
        {
        }

        public UpdatingContractStatesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
