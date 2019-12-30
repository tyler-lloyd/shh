using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Web.App.Services
{
    public interface IEncryptionService
    {
        string Encrypt(string plaintext);

        string Decrypt(string base64Ciphertext);
    }
}
