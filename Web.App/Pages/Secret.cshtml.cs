using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.App.Models;
using Web.App.Services;

namespace Web.App
{
    public class SecretModel : PageModel
    {
        private const int CodeLength = 8;

        [BindProperty] public Secret Secret { get; set; }

        public SecretModel(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        private readonly IEncryptionService _encryptionService;

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var identifier = Secret.Passphrase.ToLower() == "decrypt"
                ? _encryptionService.Decrypt(Secret.Content)
                : _encryptionService.Encrypt(Secret.Content);

            ViewData["identifier"] = identifier;

            return Page();
        }

        private static string GetUnique() => Guid.NewGuid().ToString().Substring(0, CodeLength);
    }
}