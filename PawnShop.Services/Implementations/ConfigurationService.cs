using PawnShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace PawnShop.Services.Implementations
{
    public class ConfigurationService : IConfigurationService
    {
        public T GetValueFromAppConfig<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(key));

            var value = ConfigurationManager.AppSettings.Get(key);

            return value == null ? throw new KeyNotFoundException($"Nie znaleziono wartości dla klucza: {key} w App.cfg") : (T)Convert.ChangeType(value, typeof(T));
        }

        public void SaveValueInAppConfig(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Value cannot be null or empty.", nameof(key));

            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            settings[key].Value = value;
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}
