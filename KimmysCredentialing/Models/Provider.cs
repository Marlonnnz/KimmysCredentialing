using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KimmysCredentialing.Models
{
    public class Provider
    {
        public int ProviderId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NPI { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public List<Credential> Credentials { get; set; } = new();
    }
}
