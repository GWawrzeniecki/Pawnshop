﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PawnShop.Services
{
    public class SecretManagerService : ISecretManagerService
    {
        public bool GetValue<T>(string key, out string value) where T : class
        {
            var config = new ConfigurationBuilder().AddUserSecrets<T>().Build();
            var secretProvider = config.Providers.First();

            if (secretProvider == null)
                throw new NullReferenceException("secret Provider was null");

            return secretProvider.TryGet(key, out value);
        }
    }
}
