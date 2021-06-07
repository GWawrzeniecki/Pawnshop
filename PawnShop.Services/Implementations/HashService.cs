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
        private readonly IAesService _aesService;

        #endregion private members

        #region constructor

        public HashService(ISecretManagerService secretManagerService, IAesService aesService)
        {
            this._secretManagerService = secretManagerService;
            this._aesService = aesService;
        }

        #endregion constructor

        #region public methods

        public string Hash(SecureString password)
        {
            if (password == null || password.Length == 0)
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));

            var salt = GenerateSalt();

            GetSecret(IterationsKeySecret, out string Iterations);
            var iterations = int.Parse(Iterations);

            var key = Convert.ToBase64String(DeriveKey(password, salt, iterations, KeySize));

            GetSecret(PepperAesKeySecret, out string AesPepperKey);

            var encryptedKey = Encrypt(AesPepperKey, key);
            return $"{Convert.ToBase64String(salt)}.{encryptedKey}";
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
            var key = Convert.FromBase64String(parts[1]);

            GetSecret(PepperAesKeySecret, out string AesPepperKey);

            var decryptedKey = Decrypt(AesPepperKey, Convert.ToBase64String(key));
            key = Convert.FromBase64String(decryptedKey);
            GetSecret(IterationsKeySecret, out string Iterations);
            var iterations = int.Parse(Iterations);
            var keyToCheck = DeriveKey(password, salt, iterations, KeySize);

            var verified = keyToCheck.SequenceEqual(key);

            return verified;
        }

        #endregion public methods

        #region private methods

        private byte[] DeriveKey(SecureString password, byte[] salt, int iterations, int keyByteLength)
        {
            IntPtr ptr = Marshal.SecureStringToBSTR(password);
            byte[] passwordByteArray = null;
            try
            {
                int length = Marshal.ReadInt32(ptr, -4);
                passwordByteArray = new byte[length];
                GCHandle handle = GCHandle.Alloc(passwordByteArray, GCHandleType.Pinned);
                try
                {
                    for (int i = 0; i < length; i++)
                    {
                        passwordByteArray[i] = Marshal.ReadByte(ptr, i);
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

        private byte[] GenerateSalt()
        {
            using RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltSize];
            provider.GetBytes(salt);
            return salt;
        }

        private void GetSecret(string key, out string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace", nameof(key));

            if (!_secretManagerService.GetValue<HashService>(key, out value))
                throw new Exception($"Couldn't find {key} secret key.");
        }

        private string Encrypt(string key, string valueToEncrypt) => _aesService.EncryptString(key, valueToEncrypt);

        private string Decrypt(string key, string valueToDecrypt) => _aesService.DecryptString(key, valueToDecrypt);

        #endregion private methods
    }
}