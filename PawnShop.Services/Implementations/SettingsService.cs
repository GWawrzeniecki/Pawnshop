using Newtonsoft.Json;
using PawnShop.Services.Interfaces;
using System;
using System.IO;

namespace PawnShop.Services.Implementations
{
    public class SettingsService<T> : ISettingsService<T> where T : class
    {
        private readonly string _filePath;

        public SettingsService(string fileName)
        {
            _filePath = GetLocalFilePath(fileName);
        }

        public T LoadSettings()
        {
            return File.Exists(_filePath) ?
                  JsonConvert.DeserializeObject<T>(File.ReadAllText(_filePath)) :
                  null;
        }

        public void SaveSettings(T settings)
        {
            string json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(_filePath, json);
        }

        public bool IsSettingsExist()
        {
            return File.Exists(_filePath);
        }

        private static string GetLocalFilePath(string fileName)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, fileName);
        }
    }
}