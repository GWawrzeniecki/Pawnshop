using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class LoadingUnitMeasuresException : Exception
    {
        public LoadingUnitMeasuresException()
        {
        }

        public LoadingUnitMeasuresException(string message) : base(message)
        {
        }

        public LoadingUnitMeasuresException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}