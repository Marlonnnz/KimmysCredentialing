using KimmysCredentialing.Data;
using KimmysCredentialing.Models


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace KimmysCredentialing.Services
{
    public class OfficeService
    {
        public List<Office> GetAllOffices()
        {
            using var db = new AppDbContext();

            return db.Offices
                .OrderBy(o => o.Name)
                .ToList();
        }

        public void AddOffice(Office office)
        {
            using var db = new AppDbContext();

            db.Offices.Add(office);
            db.SaveChanges();
        }

        public void UpdateOffice(Office updatedOffice)
        {
            using var db = new AppDbContext();

            var existing = db.Offices.FirstOrDefault(o => o.OfficeId == updatedOffice.OfficeId);

            if (existing is null)
                return;

            existing.Name = updatedOffice.Name;
            existing.NPI = updatedOffice.NPI;
            existing.OfficeManager = updatedOffice.OfficeManager;
            existing.Providers = updatedOffice.Providers;
            existing.Location = updatedOffice.Location;
            existing.Address1 = updatedOffice.Address1;
            existing.Address2 = updatedOffice.Address2;
            existing.City = updatedOffice.City;
            existing.State = updatedOffice.State;
            existing.ZipCode = updatedOffice.ZipCode;
            existing.Phone = updatedOffice.Phone;
            existing.Email = updatedOffice.Email;

            db.SaveChanges();
        }

        public void DeleteOffice(int officeId)
        {
            using var db = new AppDbContext();

            var office = db.Offices.FirstOrDefault(o => o.OfficeId == officeId);

            if (office is null)
                return;

            db.Offices.Remove(office);
            db.SaveChanges();
        }
    }
}
