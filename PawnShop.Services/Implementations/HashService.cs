using PawnShop.Services;
using PawnShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace PawnShop.Services.Implementations
{
    public sealed class HashService : IHashService
    {
        #region private members
        private const int SaltSize = 16; // 128 bit 
        private const int KeySize = 32; // 256 bit
        private const int Iterations = 10000;
        private readonly ISecretManagerService _secretManagerService;
        private readonly IAesService _aesService;
        #endregion


        #region constructor
        public HashService(ISecretManagerService secretManagerService, IAesService aesService)
        {
            this._secretManagerService = secretManagerService;
            this._aesService = aesService;
        }
        #endregion

        #region public methods

        public string Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));

            var salt = GenerateSalt();
            using var algorithm = new Rfc2898DeriveBytes(
               password,
               salt,
               Iterations);

            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));

            GetSecret(Constants.PepperAesKeySecret, out string AesPepperKey);

            var encryptedKey = Encrypt(AesPepperKey, key);
            return $"{Iterations}.{Convert.ToBase64String(salt)}.{encryptedKey}";
        }



        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
                throw new FormatException($"Parameter {nameof(hash)} has invalid format.");


            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);


            GetSecret(Constants.PepperAesKeySecret, out string AesPepperKey);


            var decryptedKey = Decrypt(AesPepperKey, Convert.ToBase64String(key));
            key = Convert.FromBase64String(decryptedKey);

            using var algorithm = new Rfc2898DeriveBytes(
              password,
              salt,
              iterations);
            var keyToCheck = algorithm.GetBytes(KeySize);

            var verified = keyToCheck.SequenceEqual(key);

            return verified;
        }
        #endregion

        #region private methods

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
                throw new Exception($"Couldn't find {key}.");
        }

        private string Encrypt(string key, string valueToEncrypt) => _aesService.EncryptString(key, valueToEncrypt);

        private string Decrypt(string key, string valueToDecrypt) => _aesService.DecryptString(key, valueToDecrypt);

        #endregion
    }
}
