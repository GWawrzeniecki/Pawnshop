using Microsoft.Extensions.Configuration;
using PawnShop.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace PawnShop.Core.Services.Implementations
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IConfigurationBuilder _builder;
        private readonly IConfigurationRoot _configuration;

        public UserSettingsService()
        {
            _builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("usersettings.json", optional: false, reloadOnChange: true);
            _configuration = _builder.Build();
        }
        public T GetValue<T>(string key)
        {
            var keySection = _configuration.GetSection(key);

            if (keySection is null)
                throw new KeyNotFoundException($"Nie znaleziono wartości dla klucza: {key} w usersettings.json");

            return (T)Convert.ChangeType(keySection.Value, typeof(T));
        }

        public void SaveValue(string key, string value)
        {
            AddOrUpdateAppSetting(key, value);

            _configuration.Reload();
        }



        private static void AddOrUpdateAppSetting<T>(string sectionPathKey, T value)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "usersettings.json");
            string json = File.ReadAllText(filePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            SetValueRecursively(sectionPathKey, jsonObj, value);

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, output);

        }

        private static void SetValueRecursively<T>(string sectionPathKey, dynamic jsonObj, T value)
        {
            // split the string at the first ':' character
            var remainingSections = sectionPathKey.Split(":", 2);

            var currentSection = remainingSections[0];
            if (remainingSections.Length > 1)
            {
                // continue with the procress, moving down the tree
                var nextSection = remainingSections[1];
                SetValueRecursively(nextSection, jsonObj[currentSection], value);
            }
            else
            {
                // we've got to the end of the tree, set the value
                jsonObj[currentSection] = value;
            }
        }
    }
}