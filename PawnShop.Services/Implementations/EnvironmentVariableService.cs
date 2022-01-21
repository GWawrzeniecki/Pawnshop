using PawnShop.Services.Interfaces;
using System;

namespace PawnShop.Services.Implementations
{
    public class EnvironmentVariableService : IEnvironmentVariableService
    {
        public bool TryToGetValue(string name, out string value)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            value = System.Environment.GetEnvironmentVariable(name,EnvironmentVariableTarget.User);

            return value is not null;
        }
    }
}