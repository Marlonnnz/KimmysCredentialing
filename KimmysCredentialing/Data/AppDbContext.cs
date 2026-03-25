using KimmysCredentialing.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KimmysCredentialing.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Provider> Providers => Set<Provider>();
        public DbSet<Credential> Credentials => Set<Credential>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=credentialing.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Provider>()
                .HasMany(p => p.Credentials)
                .WithOne(c => c.Provider)
                .HasForeignKey(c => c.ProviderId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
