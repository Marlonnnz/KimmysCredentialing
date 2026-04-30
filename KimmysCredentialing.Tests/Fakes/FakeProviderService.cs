using System;
using System.Collections.Generic;
using System.Text;
using KimmysCredentialing.Models;
using KimmysCredentialing.Services;

namespace KimmysCredentialing.Tests.Fakes
{
    public class FakeProviderService : IProviderService
    {
        public List<Provider> Providers { get; set; } = new();

        public bool AddProviderWasCalled { get; private set; }

        public List<Provider> GetAllProviders()
        {
            return Providers;
        }

        public List<Provider> GetAllProvidersWithCredentials()
        {
            return Providers;
        }

        public void AddProvider(Provider provider)
        {
            AddProviderWasCalled = true;
            Providers.Add(provider);
        }

        public void UpdateProvider(Provider provider)
        {

        }

        public void DeleteProvider(int providerId)
        {

        }

        public void RestoreBackup(List<Provider> providers)
        {
            Providers.Clear();
            Providers.AddRange(providers);
        }
    }
}
