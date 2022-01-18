using PawnShop.Services.Interfaces;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using static PawnShop.Core.Constants.Constants;

namespace PawnShop.Services.Implementations
{
    public sealed class HashService : IHashService
    {
        #region private members

        private const int SaltSize = 16; // 128 bit
        private const int KeySize = 32; // 256 bit
        private readonly ISecretManagerService _secretManagerService;
        private readonly IEnvironmentVariableService _environmentVariableService;

        #endregion private members

        #region constructor

        public HashService(ISecretManagerService secretManagerService, IEnvironmentVariableService environmentVariableService)
        {
            _secretManagerService = secretManagerService;
            _environmentVariableService = environmentVariableService;
        }

        #endregion constructor

        #region public methods

        public string Hash(SecureString password)
        {
            if (password == null || password.Length == 0)
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));

            var salt = GenerateSalt();

            GetEnvironmentVariable(IterationsKeySecret, out string iterations);

            var parsedIterations = int.Parse(iterations);

            var hashedPassword = Convert.ToBase64String(HashPassword(PepperPassword(password), salt, parsedIterations, KeySize));

            return $"{Convert.ToBase64String(salt)}.{hashedPassword}";
        }

        public bool Check(string hash, SecureString password)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException($"'{nameof(hash)}' cannot be null or whitespace", nameof(hash));

            if (password == null || password.Length == 0)
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));

            var parts = hash.Split('.', 2);

            if (parts.Length != 2)
                throw new FormatException($"Parameter {nameof(hash)} has invalid format.");

            var salt = Convert.FromBase64String(parts[0]);
            var originalHash = Convert.FromBase64String(parts[1]);

            GetEnvironmentVariable(IterationsKeySecret, out string iterations);

            var parsedIterations = int.Parse(iterations);
            var hashedPassword = HashPassword(PepperPassword(password), salt, parsedIterations, KeySize);

            var verified = hashedPassword.SequenceEqual(originalHash);

            return verified;
        }

        #endregion public methods

        #region private methods

        /// <summary>
        /// Hashing password with Rfc2898DeriveBytes, while keeping the password in plain text in the memory for the shortest amount of time
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iterations"></param>
        /// <param name="keyByteLength"></param>
        /// <returns></returns>
        private static byte[] HashPassword(SecureString password, byte[] salt, int iterations, int keyByteLength)
        {
            // ptr is a pointer pointing to the first character of the data string with a four byte length prefix.
            // A four - byte integer that contains the number of bytes in the following data string.
            // It appears immediately before the first character of the data string. This value does not include the terminator.
            //https://www.py4u.net/discuss/771461
            IntPtr ptr = Marshal.SecureStringToBSTR(password);
            byte[] passwordByteArray = null;
            try
            {
                int length = Marshal.ReadInt32(ptr, -4); // -4 before a pointer to the string
                passwordByteArray = new byte[length];
                //if the byte[] is not pinned then the garbage collector could relocate the object during collection and we would be left with no way to zero out the original copy.
                GCHandle handle = GCHandle.Alloc(passwordByteArray, GCHandleType.Pinned);
                try
                {
                    for (int i = 0; i < length; i++)
                    {
                        passwordByteArray[i] = Marshal.ReadByte(ptr, i); // we start from 0 = pointer to the string
                    }

                    using var rfc2898 = new Rfc2898DeriveBytes(passwordByteArray, salt, iterations);
                    return rfc2898.GetBytes(keyByteLength);
                }
                finally
                {
                    Array.Clear(passwordByteArray, 0, passwordByteArray.Length);
                    handle.Free();
                }
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
        }

        private static byte[] GenerateSalt()
        {
            using RNGCryptoServiceProvider provider = new();
            byte[] salt = new byte[SaltSize];
            provider.GetBytes(salt);
            return salt;
        }

        private void GetSecret(string key, out string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace", nameof(key));

            if (!_secretManagerService.TryToGetValue<HashService>(key, out value))
                throw new Exception($"Couldn't find {key} secret key.");
        }

        private void GetEnvironmentVariable(string name, out string value)
        {
            if (!_environmentVariableService.TryToGetValue(name, out value))
                throw new Exception($"Couldn't find {name} environment variable.");
        }

        private SecureString PepperPassword(SecureString password)
        {
            GetEnvironmentVariable(PepperKeySecret, out var pepper);

            foreach (var c in pepper.ToCharArray())
            {
                password.AppendChar(c);
            }

            return password;
        }

        #endregion private methods
    }
}