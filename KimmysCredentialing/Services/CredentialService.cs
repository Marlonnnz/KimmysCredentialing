using System.Linq;
using System.Collections.Generic;
using KimmysCredentialing.Data;
using KimmysCredentialing.Models;
using Avalonia.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace KimmysCredentialing.Services
{
    public class CredentialService
    {
        public List<Credential> GetCredentialsByProviderId(int providerId)
        {
            using var db = new AppDbContext();

            return db.Credentials
                .Include(c => c.Provider)
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

        public void DeleteCredential(int credentialId)
        {
            using var db = new AppDbContext();

            var credential = db.Credentials.FirstOrDefault(c => c.CredentialId == credentialId);

            if (credential is null)
                return;

            db.Credentials.Remove(credential);
            db.SaveChanges();
        }

        public void UpdateCredential(Credential updatedCredential)
        {
            using var db = new AppDbContext();

            var exisiting = db.Credentials.FirstOrDefault(c => c.CredentialId == updatedCredential.CredentialId);

            if (exisiting is null)
                return;

            exisiting.Name = updatedCredential.Name;
            exisiting.IssueDate = updatedCredential.IssueDate;
            exisiting.ExpirationDate = updatedCredential.ExpirationDate;
            exisiting.FilePath = updatedCredential.FilePath;
            exisiting.Notes = updatedCredential.Notes;

            db.SaveChanges();
        }
    }
}
