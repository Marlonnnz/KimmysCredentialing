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
        public string ProviderDisplayName => Provider?.Name ?? "Unkown Provider";

        public string Status
        {
            get
            {
                if (!ExpirationDate.HasValue)
                    return "No Expiration Date";

                var today = DateTime.Today;

                if(ExpirationDate.Value.Date < today)
                {
                    return "Expired";
                }

                if (ExpirationDate.Value.Date <= today.AddDays(30))
                    return "Expiring Soon";

                return "Active";
            }
        }
    }
}
