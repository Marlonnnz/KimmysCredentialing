using System.Linq;
using System.Collections.Generic;
using KimmysCredentialing.Data;
using KimmysCredentialing.Models;
using Avalonia.Data.Core;

namespace KimmysCredentialing.Services
{
    public class CredentialService
    {
        public List<Credential> GetCredentialsByProviderId(int providerId)
        {
            using var db = new AppDbContext();

            return db.Credentials
                .Where(c => c.ProviderId == providerId)
                .OrderBy(c => c.ExpirationDate)
                .ToList();
        }

        public void AddCredential(Credential credential)
        {
            using var db = new AppDbContext();

            db.Credentials.Add(credential);
            db.SaveChanges();
        }
    }
}
