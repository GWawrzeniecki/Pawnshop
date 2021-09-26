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
            _secretManagerService = secretManagerService;
            _aesService = aesService;
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

            var hashedPassword = Convert.ToBase64String(HashPassword(password, salt, iterations, KeySize));

            GetSecret(PepperAesKeySecret, out string aesPepperKey);

            var encryptedKey = Encrypt(aesPepperKey, hashedPassword);
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
            var originalHash = Convert.FromBase64String(parts[1]);

            GetSecret(PepperAesKeySecret, out string aesPepperKey);

            var decryptedHash = Decrypt(aesPepperKey, Convert.ToBase64String(originalHash));
            originalHash = Convert.FromBase64String(decryptedHash);
            GetSecret(IterationsKeySecret, out string Iterations);
            var iterations = int.Parse(Iterations);
            var hashedPassword = HashPassword(password, salt, iterations, KeySize);

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

            if (!_secretManagerService.GetValue<HashService>(key, out value))
                throw new Exception($"Couldn't find {key} secret key.");
        }

        private string Encrypt(string key, string valueToEncrypt) => _aesService.EncryptString(key, valueToEncrypt);

        private string Decrypt(string key, string valueToDecrypt) => _aesService.DecryptString(key, valueToDecrypt);



        #endregion private methods
    }
}