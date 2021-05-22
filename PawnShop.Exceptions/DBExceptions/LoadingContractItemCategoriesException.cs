using System;

namespace PawnShop.Exceptions.DBExceptions
{
    public class LoadingContractItemCategoriesException : Exception
    {
        public LoadingContractItemCategoriesException()
        {
        }

        public LoadingContractItemCategoriesException(string message) : base(message)
        {
        }

        public LoadingContractItemCategoriesException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}