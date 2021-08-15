using PawnShop.Services.Extensions;
using PawnShop.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace PawnShop.Services.Implementations
{
    public class AesService : IAesService
    {
        #region public methods

        public string EncryptString(string key, string plainText)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));

            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException($"'{nameof(plainText)}' cannot be null or empty.", nameof(plainText));

            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key.HexStringToByte().ToArray();
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }

        public string DecryptString(string key, string cipherText)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));

            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException($"'{nameof(cipherText)}' cannot be null or empty.", nameof(cipherText));

            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using Aes aes = Aes.Create();
            aes.Key = key.HexStringToByte().ToArray();
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new(buffer);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);
            return streamReader.ReadToEnd();
        }

        #endregion public methods
    }
}