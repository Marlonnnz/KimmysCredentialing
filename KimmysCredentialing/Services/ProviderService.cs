using System.Collections.Generic;
using System.Linq;
using KimmysCredentialing.Data;
using KimmysCredentialing.Models;

namespace KimmysCredentialing.Services;

public class ProviderService
{
    public List<Provider> GetAllProviders()
    {
        using var db = new AppDbContext();
        return db.Providers
            .OrderBy( p => p.Name)
            .ToList();

    }

    public void AddProvider(Provider provider)
    {
        using var db = new AppDbContext();
        db.Providers.Add(provider);
        db.SaveChanges();
    }
}