using System;
using System.Collections.Generic;
using System.Text;

namespace KimmysCredentialing.Models
{
    public class Credential
    {
        public int CredentialId { get; set; }
        public int ProviderId { get; set; }

        public string Name { get; set; } = string.Empty;
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public Provider? Provider { get; set; }
    }
}
