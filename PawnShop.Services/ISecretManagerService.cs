using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Services
{
    public interface ISecretManagerService
    {
        bool GetValue<T>(string key, out string value) where T : class;
    }
}
