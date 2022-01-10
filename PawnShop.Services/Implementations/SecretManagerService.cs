using Microsoft.Extensions.Configuration;
using PawnShop.Services.Interfaces;
using System;
using System.Linq;

namespace PawnShop.Services.Implementations
{
    public class SecretManagerService : ISecretManagerService
    {
        public bool TryToGetValue<T>(string key, out string value) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace", nameof(key));

            var config = new ConfigurationBuilder().AddUserSecrets<T>().Build();
            var secretProvider = config.Providers.First();

            if (secretProvider == null)
                throw new NullReferenceException($"Couldn't find secretProvider for {nameof(SecretManagerService)}.");

            return secretProvider.TryGet(key, out value);
        }
    }
}