using PawnShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace PawnShop.Services
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
            var salt = GenerateSalt();
            using var algorithm = new Rfc2898DeriveBytes(
               password,
               salt,
               Iterations,
               HashAlgorithmName.SHA512);

            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            GetSecret("PepperAesKey", out string AesPepperKey);
            var encryptedKey = Encrypt(AesPepperKey, key);
            return $"{Iterations}.{Convert.ToBase64String(salt)}.{encryptedKey}";
        }



        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new FormatException("Invalid hash format");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);


            GetSecret("PepperAesKey", out string AesPepperKey);
            var decryptedKey = Decrypt(AesPepperKey, Convert.ToBase64String(key));
            key = Convert.FromBase64String(decryptedKey);

            using var algorithm = new Rfc2898DeriveBytes(
              password,
              salt,
              iterations,
              HashAlgorithmName.SHA512);
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

        private bool GetSecret(string key, out string value)
        {
            return _secretManagerService.GetValue<HashService>(key, out value);
        }

        private string Encrypt(string key, string valueToEncrypt)
        {
            return _aesService.EncryptString(key, valueToEncrypt);
        }

        private string Decrypt(string key, string valueToDecrypt)
        {
            return _aesService.DecryptString(key, valueToDecrypt);
        }

        #endregion
    }
}
