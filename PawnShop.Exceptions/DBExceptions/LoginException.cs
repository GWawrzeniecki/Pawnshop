using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Exceptions.DBExceptions
{
    public class LoginException : Exception
    {
        public LoginException()
        {
        }

        public LoginException(string message) : base(message)
        {
        }

        public LoginException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
