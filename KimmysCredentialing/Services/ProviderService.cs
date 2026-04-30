using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Platform;
using KimmysCredentialing.Data;
using KimmysCredentialing.Models;
using Microsoft.EntityFrameworkCore;

namespace KimmysCredentialing.Services;

public class ProviderService : IProviderService
{
    public List<Provider> GetAllProviders()
    {
        using var db = new AppDbContext();
        return db.Providers
            .OrderBy( p => p.Name)
            .ToList();

    }

    public List<Provider> GetAllProvidersWithCredentials()
    {
        using var db = new AppDbContext();

        return db.Providers
            .Include(p => p.Credentials)
            .OrderBy(p => p.Name)
            .ToList();
    }

    public void AddProvider(Provider provider)
    {
        using var db = new AppDbContext();
        db.Providers.Add(provider);
        db.SaveChanges();
    }

    public void UpdateProvider(Provider updatedProvider)
    {
        using var db = new AppDbContext();

        var exisiting = db.Providers.FirstOrDefault(p => p.ProviderId == updatedProvider.ProviderId);

        if (exisiting is null)
            return;

        exisiting.Name = updatedProvider.Name;
        exisiting.NPI = updatedProvider.NPI;
        exisiting.Specialty = updatedProvider.Specialty;
        exisiting.Email = updatedProvider.Email;
        exisiting.Phone = updatedProvider.Phone;

        db.SaveChanges();
    }

    public void DeleteProvider(int providerId)
    {
        using var db = new AppDbContext();

        var provider = db.Providers.FirstOrDefault(p => p.ProviderId == providerId);

        if (provider is null)
            return;

        db.Providers.Remove(provider);
        db.SaveChanges();
    }

    public void RestoreBackup(List<Provider> providers)
    {
        using var db = new AppDbContext();

        db.Credentials.RemoveRange(db.Credentials);
        db.Providers.RemoveRange(db.Providers);
        db.SaveChanges();

        foreach(var provider in  providers)
        {
            provider.ProviderId = 0;

            foreach (var credential in provider.Credentials)
            {
                credential.CredentialId = 0;
                credential.Provider = null;
            }
        }

        db.Providers.AddRange(providers);
        db.SaveChanges();
    }
}

    