using System;
using System.Collections.Generic;
using System.Text;
using KimmysCredentialing.Models;

namespace KimmysCredentialing.Services
{
    public interface IProviderService
    {
        List<Provider> GetAllProviders();
        List<Provider> GetAllProvidersWithCredentials();
        void AddProvider(Provider provider);
        void UpdateProvider(Provider provider);
        void DeleteProvider(int providerId);
        void RestoreBackup(List<Provider> providers);
    }
}
