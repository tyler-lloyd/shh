using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Web.App.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string plaintext);

        string Decrypt(string base64Ciphertext);
    }

    public class AesEncryptionService : IEncryptionService
    {
        private readonly byte[] _aesKey;
        private readonly byte[] _aesIv;
        
        /// <summary>
        /// AES Encryption Service.
        /// </summary>
        /// <param name="base64EncodedKey"></param>
        /// <param name="base64EncodedIv"></param>
        public AesEncryptionService(string base64EncodedKey, string base64EncodedIv)
        {
            _aesKey = Convert.FromBase64String(base64EncodedKey);
            _aesIv = Convert.FromBase64String(base64EncodedIv);
        }

        /// <summary>
        /// Returns the base64 encoded string of the encrypted plaintext.
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public string Encrypt(string plaintext)
        {
            using (var aes = Aes.Create())
            {
                var encryptor = aes.CreateEncryptor(_aesKey, _aesIv);

                using var memStream = new MemoryStream();
                using var csStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write);
                using var swEncrypt = new StreamWriter(csStream);
                swEncrypt.Write(plaintext);
                swEncrypt.Dispose();
                return Convert.ToBase64String(memStream.ToArray());
            }
        }

        /// <summary>
        /// Returns the decrypted content.
        /// </summary>
        /// <param name="base64Ciphertext"></param>
        /// <returns></returns>
        public string Decrypt(string base64Ciphertext)
        {
            using (var aes = Aes.Create())
            {
                var encryptor = aes.CreateDecryptor(_aesKey, _aesIv);

                using var memStream = new MemoryStream(Convert.FromBase64String(base64Ciphertext));
                using var csStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Read);
                using var swDecrypt = new StreamReader(csStream);
                return swDecrypt.ReadToEnd();
            }
        }
    }
}
