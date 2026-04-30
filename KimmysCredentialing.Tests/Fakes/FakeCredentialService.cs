using System;
using System.Collections.Generic;
using System.Text;
using KimmysCredentialing.Models;
using KimmysCredentialing.Services;

namespace KimmysCredentialing.Tests.Fakes
{
    public class FakeCredentialService : ICredentialService
    {
        public List<Credential> Credentials { get; } = new();

        public List<Credential> GetCredentialsByProviderId(int providerId)
        {
            return Credentials.Where(c => c.ProviderId == providerId).ToList();
        }

        public void AddCredential(Credential credential)
        {
            Credentials.Add(credential);
        }

        public void UpdateCredential(Credential credential)
        {

        }

        public void DeleteCredential(int credentialId)
        {

        }
    }
}
