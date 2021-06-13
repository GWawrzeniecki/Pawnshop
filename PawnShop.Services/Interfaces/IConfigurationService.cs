using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Services.Interfaces
{
    public interface IConfigurationService
    {
        public T GetValue<T>(string key);
    }
}
