using System;
using System.Collections.Generic;
using System.Text;
using KimmysCredentialing.Models;

namespace KimmysCredentialing.Services
{
    public interface ICredentialService
    {
        List<Credential> GetCredentialsByProviderId(int providerId);
        void AddCredential(Credential credential);
        void UpdateCredential(Credential credential);
        void DeleteCredential(int credentialId);
    }
}
