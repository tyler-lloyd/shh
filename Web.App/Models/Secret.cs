using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.App.Models
{
    public class Secret
    {
        public int Id { get; set; }

        public string SecretId { get; set; }

        public string Content { get; set; }

        public string Passphrase { get; set; }

        public DateTime Expiration { get; set; }

        public bool Active { get; set; }
    }
}
