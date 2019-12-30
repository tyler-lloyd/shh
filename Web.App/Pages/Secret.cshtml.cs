using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.App.Models;

namespace Web.App
{
    public class SecretModel : PageModel
    {
        private const int CodeLength = 8;

        [BindProperty] public Secret Secret { get; set; }

        public SecretModel()
        {
            InitEncryption();
        }

        private void InitEncryption()
        {
            _aesKey = Convert.FromBase64String("y00dHtcluIgXO+9VO74r9CN7QjZLyQY48MsED4e1fNQ=");
            _aesIv = Convert.FromBase64String("ttiN3IfbgmJtuW0gYTX9GQ==");
        }

        private byte[] _aesKey { get; set; }
        private byte[] _aesIv { get; set; }

        private readonly Aes _aes = Aes.Create();

        public void OnGet()
        {
            var s = new Secret();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var identifier = Secret.Passphrase.ToLower() == "decrypt"
                ? Decrypt(Secret.Content)
                : Encrypt(Secret.Content);

            ViewData["identifier"] = identifier;

            return Page();
        }

        private string Encrypt(string content)
        {
            var encryptor = _aes.CreateEncryptor(_aesKey, _aesIv);

            using var memStream = new MemoryStream();
            using var csStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write);
            using var swEncrypt = new StreamWriter(csStream);
            swEncrypt.Write(content);
            swEncrypt.Flush();
            return Convert.ToBase64String(memStream.ToArray());
        }

        private string Decrypt(string base64string)
        {
            var encryptor = _aes.CreateDecryptor(_aesKey, _aesIv);

            using var memStream = new MemoryStream(Convert.FromBase64String(base64string));
            using var csStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Read);
            using var swDecrypt = new StreamReader(csStream);
            return swDecrypt.ReadToEnd();
        }

        private static string GetUnique() => Guid.NewGuid().ToString().Substring(0, CodeLength);
    }
}