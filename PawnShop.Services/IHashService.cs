using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace PawnShop.Services
{
    public interface IHashService
    {
        string Hash(string password);
        bool  Check(string hash, string password);
    }
}
