using System;
using System.Collections.Generic;
using System.Text;
using PawnShop.Services.Interfaces;
using System.Configuration;
using System.Collections.Specialized;

namespace PawnShop.Services.Implementations
{
    public class ConfigurationService : IConfigurationService
    {
        public T GetValue<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

            var value = ConfigurationManager.AppSettings.Get(key);

            return value == null ? throw new KeyNotFoundException($"Nie znaleziono wartości dla klucza: {key} w App.cfg") : (T)Convert.ChangeType(value, typeof(T));



        }
    }
}
